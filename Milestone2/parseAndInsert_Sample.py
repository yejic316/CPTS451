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
        #outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='durwjs13'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema.
            #  Update the statement based on your own table schema and include values for all businessTable attributes
            sql_str = "INSERT INTO businessTable (business_id, name, address,state,city,zipcode,latitude,longitude,stars,reviewcount,numCheckins,openStatus, Categories) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["address"]) + "','" + \
                      cleanStr4SQL(data["state"]) + "','" + cleanStr4SQL(data["city"]) + "','" + cleanStr4SQL(data["postal_code"]) + "','" + str(data["latitude"]) + "," + \
                      str(data["longitude"]) + "," + str(data["stars"]) + "," + str(data["review_count"]) + ",0 ,"  + \
                      int2BoolStr(data["is_open"])  + ");"
                    #  '[' + (for i in data['categories'] +',') + ']'+ "','"  + \
                    #  '[' + (for i in data['attribute'] +',') + ']' + "','" + \
                    #  '[' + (for i in data['hours'] +',') + ']' + ");"
            try:
                cur.execute(sql_str)
            except:
                print("Insert to businessTABLE failed!")
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

#
#
#
# #################################################################################################
# def insert2UserTable():
#     #reading the JSON file
#     with open('./yelp_user.JSON','r') as f:    #TODO: update path for the input file
#         #outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
#         line = f.readline()
#         count_line = 0
#
#         #connect to yelpdb database on postgres server using psycopg2
#         #TODO: update the database name, username, and password
#         try:
#             conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='durwjs13'")
#         except:
#             print('Unable to connect to the database!')
#         cur = conn.cursor()
#
#         while line:
#             data = json.loads(line)
#             # Generate the INSERT statement for the cussent business
#             # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
#             # include values for all businessTable attributes
#             sql_str = "INSERT INTO userTable (user_id, name, yelping_since, review_count, fans,average_stars,funny,useful,cool,friends) " \
#                       "VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + str(data['yelping_since']) + "','" + \
#                       str(data['review_count'])+ "','" + str(data['fans']) + "','" + str(data['average_stars']) + "'," + str(data["funny"]) + "," + \
#                       str(data["useful"]) + "," + str(data["cool"]) + "," + '['+ (for i in str(data["friends"])) +']' + ");"
#             try:
#                 cur.execute(sql_str)
#             except:
#                 print("Insert to userTABLE failed!")
#             conn.commit()
#             # optionally you might write the INSERT statement to a file.
#             # outfile.write(sql_str)
#
#             line = f.readline()
#             count_line +=1
#
#         cur.close()
#         conn.close()
#
#     print(count_line)
#     #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
#     f.close()
#
#
#
#
#
#
# #################################################################################################
# def insert2CheckinTable():
#     #reading the JSON file
#     with open('./yelp_user.JSON','r') as f:    #TODO: update path for the input file
#         #outfile =  open('./yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
#         line = f.readline()
#         count_line = 0
#
#         #connect to yelpdb database on postgres server using psycopg2
#         #TODO: update the database name, username, and password
#         try:
#             conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='durwjs13'")
#         except:
#             print('Unable to connect to the database!')
#         cur = conn.cursor()
#
#         while line:
#             data = json.loads(line)
#             # Generate the INSERT statement for the cussent business
#             # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
#             # include values for all businessTable attributes
#             sql_str = "INSERT INTO checkinTable (business_id, dayofweek, morning, afternoon, evening, night) " \
#                       "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + str(data['yelping_since']) + "','" + \
#                       str(data['review_count'])+ "','" + str(data['fans']) + "','" + str(data['average_stars']) + "'," + str(data["funny"]) + "," + \
#                       str(data["useful"]) + "," + str(data["cool"]) + "," + str(data["friends"]) + ");"
#             try:
#                 cur.execute(sql_str)
#             except:
#                 print("Insert to checkinTABLE failed!")
#             conn.commit()
#             # optionally you might write the INSERT statement to a file.
#             # outfile.write(sql_str)
#
#             line = f.readline()
#             count_line +=1
#
#         cur.close()
#         conn.close()
#
#     print(count_line)
#     #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
#     f.close()
#
#
insert2BusinessTable()
# insert2UserTable()
# insert2CheckinTable()
# insert2ReviewTable()