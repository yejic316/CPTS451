import json

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def recursiveDic(dic):
    list =[]
    for i, j in dic.items():
        list.append((i,j))
    return list

def parseBusinessData():
    #read the JSON file
    with open('./yelp_business.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('business.txt', 'w')
        line = f.readline()
        count_line = 0

        

        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id'])+',') #business id
            outfile.write(cleanStr4SQL(data['name'])+', ') #name
            outfile.write(cleanStr4SQL(data['address'])+', ') #full_address
            outfile.write(cleanStr4SQL(data['state'])+', ') #state
            outfile.write(cleanStr4SQL(data['city'])+', ') #city
            outfile.write(cleanStr4SQL(data['postal_code']) + ', ')  #zipcode
            outfile.write(str(data['latitude'])+', ') #latitude
            outfile.write(str(data['longitude'])+', ') #longitude
            outfile.write(str(data['stars'])+', ') #stars
            outfile.write(str(data['review_count'])+', ') #reviewcount
            outfile.write(str(data['is_open'])+'\n') #openstatus

            ################################################### category begins
            outfile.write('Categories: '+'[')  #category list            
            for i in data['categories']:
                outfile.write(i+', ')
            outfile.write(']'+'\n')            
            ################################################### attribute begins
            attributeList=[]
            outputAtt={}
            outfile.write('Attributes: '+'[')
            for i,j in data['attributes'].items():
                if type(j) is dict:
                    attributeList=recursiveDic(j)
                    for x,y in attributeList:
                        outputAtt[x]=y
                else:
                    #outfile.write(str((i,j))+'') # write your own code to process attributes
                    outputAtt[i]=j
            attributeList=[] #empty it
            for i,j in outputAtt.items():
                outfile.write('(')
                outfile.write(i)
                outfile.write(', ')
                outfile.write(str(j))
                outfile.write(')')
                #list+=((i,j))
             # write your own code to process hours
            outfile.write(']'+'\n')
            ################################################### hours begins

            outfile.write('Hours: '+'[')
            for i,j in data['hours'].items():
                a= j.split('-')
                outfile.write('(')
                outfile.write(i)
                outfile.write(', ')
                outfile.write(a[0])
                outfile.write(',')
                outfile.write(a[1])
                outfile.write(')')
            outfile.write(']\n')
            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()
    ######################## wrote bussiness file above ##################


def parseUserData():
        # read the JSON file
    with open('./yelp_user.JSON','r') as f:  # Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile = open('user.txt', 'w')
        line = f.readline()
        count_line = 0
        # read each JSON abject and extract data
        outfile.write("user_id,  name,  yelping_since,  review_count,  fans,  average_star,  funny,  useful,  cool,  friends \n")

        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['user_id'])+',') #user id
            outfile.write(cleanStr4SQL(data['name'])+',') #name
            outfile.write(str(data['yelping_since']) + ',')  # yelping_since
            outfile.write(str(data['review_count']) + ',')  # review_count
            outfile.write(str(data['fans']) + ',')  # fan
            outfile.write(str(data['average_stars']) + ',')  # average_star
            outfile.write(str(data['funny']) + ',')  # funny
            outfile.write(str(data['useful']) + ',')  # useful
            outfile.write(str(data['cool']) + ',')  # cool
            #outfile.write("\nFriends: " + str([item for item in  data['friends']])) #friends list
            outfile.write("\nFriends: [") #friends list
            for item in data['friends']:
                outfile.write(item+' , ')
            outfile.write("]\n")  # friends list

            line = f.readline()
            count_line += 1
    print(count_line)
    outfile.close()
    f.close()


def parseCheckinData():
    
    with open('./yelp_checkin.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('checkin.txt', 'w')
        line = f.readline()
        count_line = 0
        #                                        5~10       11~16      17~22     23~4
        outfile.write('business_id,  dayofweek,  morning,  afternoon,  evening,  night\n') 

        morning=[5,6,7,8,9,10]
        afternoon=[11,12,13,14,15,16]
        evening=[17,18,19,20,21,22]
        night=[23,0,1,2,3,4]

  
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id']))
            
            for i,j in data.items(): 
                if(type(j) is dict):
                    for x,y in j.items(): # x = dayname  y = checkins with time 
                       # print(x,y) 
                        outfile.write('\n\t\t('+x+', ')
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
                        count = 0
                        #print(len(checkin_filter))
                        for i,j in checkin_filter.items():
                            if(count ==3):
                                outfile.write(str(j))
                            else:
                                outfile.write(str(j)+', ')
                            count +=1
                        outfile.write(')')
                    outfile.write('\n')
            

            '''
            outfile.write('\t\t'+cleanStr4SQL(data['dayofweek'])+'\n')
            outfile.write('\t\t'+cleanStr4SQL(data['morning'])+'\n')
            outfile.write('\t\t'+cleanStr4SQL(data['afternoon'])+'\n')
            outfile.write('\t\t'+cleanStr4SQL(data['evening'])+'\n')
            outfile.write('\t\t'+cleanStr4SQL(data['night'])+'\n')
            '''




            line = f.readline()
            count_line +=1

    print(count_line)
    outfile.close()
    f.close()


def parseReviewData():
    #read the JSON file
    with open('./yelp_review.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('review.txt', 'w')
        line = f.readline()
        count_line = 0
        outfile.write("review_id,  user_id,  business_id,  text,  stars,  date,  funny,  useful,  cool  ")

        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['review_id'])+',') #review id
            outfile.write(cleanStr4SQL(data['user_id'])+',') #user id
            outfile.write(cleanStr4SQL(data['business_id'])+',') #business id
            outfile.write(cleanStr4SQL(data['text'])+',') #text
            outfile.write(str(data['stars'])+',')
            outfile.write(str(data['date'])+',')
            outfile.write(str(data['funny'])+',') #funny
            outfile.write(str(data['useful'])+',') #useful
            outfile.write(str(data['cool'])) #cool
            outfile.write('\n')

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

parseBusinessData()
parseUserData()
parseCheckinData()
parseReviewData()
