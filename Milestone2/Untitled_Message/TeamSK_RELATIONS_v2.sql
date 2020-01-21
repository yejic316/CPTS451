
CREATE TABLE Business (
    Business_id VARCHAR(30),
    Name VARCHAR(128),
    State VARCHAR(2),
    City VARCHAR(128),
    Address VARCHAR(128),
    Zipcode INT,
    Stars FLOAT CHECK (Stars <=5.0 AND Stars >=0.0),
    Is_open INT,
    Latitude FLOAT,
    Longitude FLOAT,
    Review_count INT,
    Review_ratings FLOAT,
    Num_checkins INT,
    PRIMARY KEY (Business_id)
);

CREATE TABLE Users (
    User_id VARCHAR(30),
    Average_stars FLOAT CHECK (Average_stars <=5.0 AND Average_stars >=0.0),
    Fans INT,
    Name VARCHAR(128),
    Cool INT,
    Funny INT,
    Useful INT,
    Review_count INT,
    Yelping_since DATE,
    User_latitude FLOAT,
    User_longitude FLOAT,
    PRIMARY KEY (User_id)
);

CREATE TABLE Review(
    Review_id VARCHAR(30),
    Text VARCHAR(1024),
    Review_date DATE,
    Review_stars INT CHECK (Review_stars <=5 AND Review_stars >=0),
    Funny INT,
    Cool INT,
    Useful INT,
    Business_id VARCHAR(30) NOT NULL,
    User_id VARCHAR(30),
    PRIMARY KEY (Review_id),
    FOREIGN KEY (Business_id) REFERENCES Business(Business_id),
    FOREIGN KEY (User_id) REFERENCES Users(User_id)
);

CREATE TABLE Favorites(
    Business_id VARCHAR(30),
    User_id VARCHAR(30),
    PRIMARY KEY (Business_id,User_id),
    FOREIGN KEY (Business_id) REFERENCES Business(Business_id),
    FOREIGN KEY (User_id) REFERENCES Users(User_id)
);

CREATE TABLE Friends_with(
    User_id VARCHAR(30),
    Friends_id VARCHAR(30),
    PRIMARY KEY (User_id,Friends_id),
    FOREIGN KEY (User_id) REFERENCES Users(User_id),
    FOREIGN KEY (Friends_id) REFERENCES Users(User_id)

);

CREATE TABLE Categories(
    Business_id VARCHAR(30),
    Categories_name VARCHAR(128),
    PRIMARY KEY (Business_id,Categories_name),
    FOREIGN KEY (Business_id) REFERENCES Business(Business_id)
);

CREATE TABLE Attributes(
    Business_id VARCHAR(30),
    Attributes_name VARCHAR(128),
    Attributes_value INT,
    PRIMARY KEY (Business_id,Attributes_name),
    FOREIGN KEY (Business_id) REFERENCES Business(Business_id)
);

CREATE TABLE Hours(
    Business_id VARCHAR(30),
    Hours_day VARCHAR(10),
    Hours_open TIME,
    Hours_close TIME,
    PRIMARY KEY (Business_id,Hours_day),
    FOREIGN KEY (Business_id) REFERENCES Business(Business_id)
);

CREATE TABLE Checkin(
    Business_id VARCHAR(30),
    Checkin_day VARCHAR(10),
    Checkin_time VARCHAR(10),
    Checkin_count INT,
    PRIMARY KEY (Business_id,Checkin_day,Checkin_time),
    FOREIGN KEY (Business_id) REFERENCES Business(Business_id)  
);



