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
    with open('./yelp_business.JSON','r') as f:
        outfile =  open('./yelp_business.SQL', 'w')
        line = f.readline()
        count_line = 0
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='teamsk'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()
        while line:
            data = json.loads(line)
            sql_str = "INSERT INTO Business (Business_id, Name, State,City,Address,Zipcode,Stars,Is_open,Latitude,Longitude,Review_count,Review_ratings,Num_checkins) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["state"]) + "','" + \
                      cleanStr4SQL(data["city"]) + "','" + cleanStr4SQL(data["address"]) + "','" + cleanStr4SQL(data["postal_code"]) + "'," + str(data["stars"]) + "," + \
                      str(data["is_open"]) + "," + str(data["latitude"]) + "," + str(data["longitude"]) + "," + str(data["review_count"]) + ",0.0,0" + ");"
            try:
                cur.execute(sql_str)
            except:
                print("Insert to businessTABLE failed! \n"+sql_str)
            conn.commit()
            outfile.write(sql_str)
            line = f.readline()
            count_line +=1
        cur.close()
        conn.close()
    print(count_line)
    f.close()

def insert2Categories():
    with open('./yelp_business.JSON','r') as f:
        outfile =  open('./yelp_categories.SQL', 'w')
        line = f.readline()
        count_line = 0
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='teamsk'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()
        while line:
            data = json.loads(line)
            for category in data['categories']:
                sql_str = "INSERT INTO Categories (Business_id,Categories_name) " \
                          "VALUES ('" + data['business_id'] + "','" + cleanStr4SQL(category) + "');"
                try:
                    cur.execute(sql_str)
                except:
                    print("Insert to businessTABLE failed! \n"+sql_str)
                conn.commit()
                outfile.write(sql_str)
                count_line +=1
            line = f.readline()
        cur.close()
        conn.close()
    print(count_line)
    f.close()

def insert2Checkin():
    count =0
    with open('./yelp_checkin.JSON','r') as f:
        outfile =  open('./yelp_checkin.SQL', 'w')
        line = f.readline()
        count_line = 0
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='teamsk'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()
        while line:
            data = json.loads(line)
            for i, j in data.items():
                if (type(j) is dict):
                    for x, y in j.items():
                        for s, h in y.items():
                            sql_str = "INSERT INTO Checkin (Business_id, Checkin_day, Checkin_time,Checkin_count) " \
                                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + x+"','"+ s+"','"+str(h)+"');"
                            try:
                                cur.execute(sql_str)
                            except:
                                print("Insert to Checkin failed! \n" + sql_str)
                            conn.commit()
                            outfile.write(sql_str)
                            count_line += 1
            line = f.readline()
        cur.close()
        conn.close()
    print(count_line)
    f.close()

def insert2Hours():
    with open('./yelp_business.JSON','r') as f:
        outfile =  open('./yelp_hours.SQL', 'w')
        line = f.readline()
        count_line = 0
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='teamsk'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()
        while line:
            data = json.loads(line)
            if data['hours']!={}:
                for x,y in data['hours'].items():
                    tem=(y.split("-"))
                    sql_str = "INSERT INTO Hours (Business_id, Hours_day,Hours_open,Hours_close) " \
                              "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + x+"','"+ tem[0]+"','"+tem[1]+"');"
                    try:
                        cur.execute(sql_str)
                    except:
                        print("Insert to businessTABLE failed! \n"+sql_str)
                    conn.commit()
                    count_line +=1
                    outfile.write(sql_str)
            line = f.readline()
        cur.close()
        conn.close()
    print(count_line)
    f.close()

def insert2FriendsWith():
    with open('./yelp_user.JSON','r') as f:
        outfile =  open('./yelp_friendswith.SQL', 'w')
        line = f.readline()
        count_line = 0
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='teamsk'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()
        while line:
            data = json.loads(line)
            for i in data['friends']:
                sql_str = "INSERT INTO Friends_with (user_id, Friends_id) " \
                          "VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + i+"');"
                try:
                    cur.execute(sql_str)
                except:
                    print("Insert to Friends_with failed! \n"+sql_str)
                conn.commit()
                count_line +=1
                outfile.write(sql_str)
            line = f.readline()
        cur.close()
        conn.close()
    print(count_line)
    f.close()

def insert2UsersTable():
    with open('./yelp_user.JSON','r') as f:
        outfile =  open('./yelp_user.SQL', 'w')
        line = f.readline()
        count_line = 0
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='teamsk'")
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
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='teamsk'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()
        while line:
            data = json.loads(line)
            sql_str = "INSERT INTO Review (Review_id, Text, Review_date, Review_stars,Funny,Cool,useful,Business_id,User_id) " \
                      "VALUES ('" + cleanStr4SQL(data['review_id']) + "','" + cleanStr4SQL(data["text"]) + "','" + str(data["date"]) + "'," + \
                      str(data["stars"]) + "," + str(data["funny"]) + "," + str(data["cool"]) + "," + str(data["useful"]) + ",'" + \
                      cleanStr4SQL(data["business_id"]) + "','" + cleanStr4SQL(data["user_id"]) + "');"
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



insert2BusinessTable()
insert2Categories()
insert2Checkin()
insert2Hours()
insert2UsersTable()
insert2FriendsWith()
insert2ReviewTable()