-- MySQL dump 10.13  Distrib 8.0.13, for macos10.14 (x86_64)
--
-- Host: 127.0.0.1    Database: CPTS451
-- ------------------------------------------------------
-- Server version	8.0.13

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Attribute`
--

DROP TABLE IF EXISTS `Attribute`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `Attribute` (
  `AttributeID` varchar(22) NOT NULL,
  `Name` varchar(45) NOT NULL,
  `Bool` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`AttributeID`,`Name`),
  UNIQUE KEY `Name_UNIQUE` (`Name`),
  CONSTRAINT `AttributeId` FOREIGN KEY (`AttributeID`) REFERENCES `business` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Attribute`
--

LOCK TABLES `Attribute` WRITE;
/*!40000 ALTER TABLE `Attribute` DISABLE KEYS */;
/*!40000 ALTER TABLE `Attribute` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Business`
--

DROP TABLE IF EXISTS `Business`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `Business` (
  `ID` varchar(22) NOT NULL,
  `NAME` varchar(45) NOT NULL,
  `STATE` varchar(45) NOT NULL,
  `CITY` varchar(45) NOT NULL,
  `ADDRESS` varchar(45) NOT NULL,
  `Zip_Code` int(11) NOT NULL,
  `Longitude` decimal(7,0) NOT NULL,
  `Latitude` decimal(7,0) NOT NULL,
  `Stars` decimal(1,0) NOT NULL DEFAULT '0',
  `Review_Count` int(11) NOT NULL DEFAULT '0',
  `IS_Open` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Business`
--

LOCK TABLES `Business` WRITE;
/*!40000 ALTER TABLE `Business` DISABLE KEYS */;
/*!40000 ALTER TABLE `Business` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Checkin`
--

DROP TABLE IF EXISTS `Checkin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `Checkin` (
  `ID` varchar(22) NOT NULL,
  `DayOfWeek` varchar(45) NOT NULL,
  `Morning` int(11) NOT NULL,
  `Afternoon` int(11) NOT NULL,
  `Evening` int(11) NOT NULL,
  `Night` int(11) NOT NULL,
  PRIMARY KEY (`ID`,`DayOfWeek`),
  UNIQUE KEY `DayOfWeek_UNIQUE` (`DayOfWeek`),
  CONSTRAINT `ID` FOREIGN KEY (`ID`) REFERENCES `business` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Checkin`
--

LOCK TABLES `Checkin` WRITE;
/*!40000 ALTER TABLE `Checkin` DISABLE KEYS */;
/*!40000 ALTER TABLE `Checkin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `HoursID`
--

DROP TABLE IF EXISTS `HoursID`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `HoursID` (
  `HoursID` varchar(22) NOT NULL,
  `Day` varchar(10) NOT NULL,
  `Hour` int(11) NOT NULL,
  PRIMARY KEY (`HoursID`),
  CONSTRAINT `HoursId` FOREIGN KEY (`HoursID`) REFERENCES `business` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `HoursID`
--

LOCK TABLES `HoursID` WRITE;
/*!40000 ALTER TABLE `HoursID` DISABLE KEYS */;
/*!40000 ALTER TABLE `HoursID` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Review`
--

DROP TABLE IF EXISTS `Review`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `Review` (
  `Review_ID` varchar(22) NOT NULL,
  `Text` varchar(64) NOT NULL,
  `Date` datetime NOT NULL,
  `Stars` int(11) NOT NULL DEFAULT '0',
  `Funny` int(11) NOT NULL DEFAULT '0',
  `Cool` int(11) NOT NULL DEFAULT '0',
  `Useful` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Review_ID`),
  UNIQUE KEY `Review_ID_UNIQUE` (`Review_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Review`
--

LOCK TABLES `Review` WRITE;
/*!40000 ALTER TABLE `Review` DISABLE KEYS */;
/*!40000 ALTER TABLE `Review` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `User`
--

DROP TABLE IF EXISTS `User`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `User` (
  `ID` varchar(22) NOT NULL,
  `Name` varchar(45) NOT NULL,
  `Avg_Stars` float NOT NULL,
  `Yelping_Since` datetime NOT NULL,
  `Fans` varchar(45) DEFAULT '0',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `idUser_UNIQUE` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `User`
--

LOCK TABLES `User` WRITE;
/*!40000 ALTER TABLE `User` DISABLE KEYS */;
/*!40000 ALTER TABLE `User` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-02-07 20:50:13
