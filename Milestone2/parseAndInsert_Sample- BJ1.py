import json
import psycopg2

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def int2BoolStr (value):
    if value == 0:
        return 'False'
    else:
        return 'True'

def insert2BusinessTable():
    #reading the JSON file
    with open('./yelp_business.JSON','r') as f:    #TODO: update path for the input file
        outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='teamsk'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on lsyour own table schema and
            # include values for all businessTable attributes
            sql_str = "INSERT INTO Business (Business_id, Name, State,City,Address,Zipcode,Stars,Is_open,Latitude,Longitude,Review_count,Review_ratings,Num_checkins) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["state"]) + "','" + \
                      cleanStr4SQL(data["city"]) + "','" + cleanStr4SQL(data["address"]) + "','" + cleanStr4SQL(data["postal_code"]) + "'," + str(data["stars"]) + "," + \
                      str(data["is_open"]) + "," + str(data["latitude"]) + "," + str(data["longitude"]) + "," + str(data["review_count"]) + ",0.0,0" +");"

            try:
                cur.execute(sql_str)
            except:
                #print("Insert to businessTABLE failed!"+ " " + data['business_id'] + " " + data['name'] +" "+ data['state'] + " " + data['city'] +" ")
                print("Insert to businessTABLE failed! \n"+sql_str)
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


insert2BusinessTable()