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
    outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2Categories():
    #reading the JSON file
    with open('./yelp_business.JSON','r') as f:    #TODO: update path for the input file
        outfile =  open('./yelp_categories.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
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
            for category in data['categories']:
                sql_str= "INSERT INTO Categories (Business_id,Categories_name) "\
                    "VALUES ('"+data['business_id']+"','"+cleanStr4SQL(category)+"');"
                #print(sql_str)
            #return
                try:
                    cur.execute(sql_str)
                except:
                    #print("Insert to businessTABLE failed!"+ " " + data['business_id'] + " " + data['name'] +" "+ data['state'] + " " + data['city'] +" ")
                    print("Insert to CategoriesTable failed! \n"+sql_str)
                conn.commit()
                # optionally you might write the INSERT statement to a file.
                outfile.write(sql_str)
                count_line +=1
            line = f.readline()
            

        cur.close()
        conn.close()

    print(count_line)
    outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2Checkin():


    morning=[5,6,7,8,9,10]
    afternoon=[11,12,13,14,15,16]
    evening=[17,18,19,20,21,22]
    night=[23,0,1,2,3,4]
    count =0

    #reading the JSON file
    with open('./yelp_checkin.JSON','r') as f:    #TODO: update path for the input file
        outfile =  open('./yelp_checkin.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
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

            for i,j in data.items(): 
                
                if(type(j) is dict):
                    for x,y in j.items(): # x = dayname  y = checkins with time 
                        checkin_filter = { 'morning' :0, 'afternoon':0, 'evening':0, 'night':0}
                        for s,h in y.items():
                            
                            timeVar=s.split(":")
                            #print(timeVar[0],h)
                            
                            if int(timeVar[0]) in morning:
                                checkin_filter["morning"]+=int(h)
                            if int(timeVar[0]) in afternoon:
                                checkin_filter["afternoon"]+=int(h)
                            if int(timeVar[0]) in evening:
                                checkin_filter["evening"]+=int(h)
                            if int(timeVar[0]) in night:
                                checkin_filter["night"]+=int(h)
                        #print(data['business_id'],"  ", x ,"  ",checkin_filter)
                        for i,j in checkin_filter.items():
                            #print(data['business_id'],"  ", x ,"  ",i,j)
                            sql_str = "INSERT INTO Checkin (Business_id, Checkin_day, Checkin_time,Checkin_count) " \
                            "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + x+"','"+ i+"','"+str(j)+"');"
                            try:
                                cur.execute(sql_str)
                            except:
                                print("Insert to Checkin failed! \n"+sql_str)
                            conn.commit()
                            outfile.write(sql_str)
                            count_line +=1
                   # print("\n")
                #print(data['business_id'],x,checkin_filter)
        

            line = f.readline()
        cur.close()
        conn.close()

    print(count_line)
    outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2Hours():
    #reading the JSON file
    with open('./yelp_business.JSON','r') as f:    #TODO: update path for the input file
        outfile =  open('./yelp_hours.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
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
            if data['hours']!={}:
                for x,y in data['hours'].items():
                    tem=(y.split("-"))
                    #print(data['business_id'],x,tem[0],tem[1])
                    sql_str = "INSERT INTO Hours (Business_id, Hours_day,Hours_open,Hours_close) " \
                            "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + x+"','"+ tem[0]+"','"+tem[1]+"');"
                    try:
                        cur.execute(sql_str)
                    except:
                        print("Insert to businessTABLE failed! \n"+sql_str)
                    conn.commit()
                    # optionally you might write the INSERT statement to a file.
                    count_line +=1
                    outfile.write(sql_str)
            line = f.readline()
        cur.close()
        conn.close()
    print(count_line)
    outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


def insert2FriendsWith():
    #reading the JSON file
    with open('./yelp_user.JSON','r') as f:    #TODO: update path for the input file
        outfile =  open('./yelp_friendswith.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
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
            for i in data['friends']:
                #print(data['user_id']," ",i)
                sql_str = "INSERT INTO Friends_with (user_id, Friends_id) " \
                            "VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + i+"');"
                try:
                        cur.execute(sql_str)
                except:
                    print("Insert to Friends_with failed! \n"+sql_str)
                conn.commit()
                # optionally you might write the INSERT statement to a file.
                count_line +=1
                outfile.write(sql_str)
            line = f.readline()
        cur.close()
        conn.close()
    print(count_line)
    outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()



def insert2UsersTable():
    with open('./yelp_user.JSON','r') as f:
        outfile =  open('./yelp_user.SQL', 'w')
        line = f.readline()
        count_line = 0
        try:
            conn = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='teamsk'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            sql_str = "INSERT INTO Users (User_id, Average_stars, Fans, Name, Cool,Funny,Useful,Review_count,Yelping_since, User_latitude, User_longitude) " \
                      "VALUES ('" + cleanStr4SQL(data['user_id']) + "'," + str(data["average_stars"]) + "," + str(data["fans"]) + ",'" + \
                      cleanStr4SQL(data["name"]) + "'," + str(data["cool"]) + "," + str(data["funny"]) + "," + str(data["useful"]) + "," + \
                      str(data["review_count"]) + ",'" + str(data["yelping_since"])  + "',0.0,0.0" +");"
            try:
                cur.execute(sql_str)
            except:
                print("Insert to usersTable failed! \n"+sql_str)
            conn.commit()
            outfile.write(sql_str)
            line = f.readline()
            count_line +=1
        cur.close()
        conn.close()
    print(count_line)
    f.close()


def insert2ReviewTable():
    with open('./yelp_review.JSON','r') as f:
        outfile =  open('./yelp_review.SQL', 'w')
        line = f.readline()
        count_line = 0
        try:
            conn = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='teamsk'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()
        while line:
            data = json.loads(line)
            sql_str = "INSERT INTO Review (Review_id, Text, Review_date, Review_stars,Funny,Cool,useful,Business_id,User_id) " \
                      "VALUES ('" + cleanStr4SQL(data['review_id']) + "','" + cleanStr4SQL(data["text"]) + "','" + str(data["date"]) + "'," + \
                      str(data["stars"]) + "," + str(data["funny"]) + "," + str(data["cool"]) + "," + str(data["useful"]) + ",'" + \
                      cleanStr4SQL(data["business_id"]) +"','"+cleanStr4SQL(data["user_id"])+"');"
            try:
                cur.execute(sql_str)
            except:
                print("Insert to ReviewTable failed! \n"+sql_str)
            conn.commit()
            outfile.write(sql_str)
            line = f.readline()
            count_line +=1
        cur.close()
        conn.close()
    print(count_line)
    f.close()




"""
insert2BusinessTable()
insert2Categories()
insert2Checkin()
insert2Hours()
insert2UsersTable()
insert2FriendsWith()
insert2ReviewTable()
"""
insert2ReviewTable()