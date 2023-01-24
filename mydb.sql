-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
-- Host: localhost    Database: mydb
-- ------------------------------------------------------
-- Server version	8.0.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `administrator`
--

DROP TABLE IF EXISTS `administrator`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `administrator` (
  `idAdministrator` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`idAdministrator`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `administrator`
--

LOCK TABLES `administrator` WRITE;
/*!40000 ALTER TABLE `administrator` DISABLE KEYS */;
INSERT INTO `administrator` VALUES (1);
/*!40000 ALTER TABLE `administrator` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kierowcy`
--

DROP TABLE IF EXISTS `kierowcy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kierowcy` (
  `idKierowcy` int NOT NULL AUTO_INCREMENT,
  `nazwisko_prac` varchar(45) DEFAULT NULL,
  `imie_prac` varchar(45) DEFAULT NULL,
  `numer_tel_kierowcy` varchar(9) DEFAULT NULL,
  `czy_aktywne` tinyint DEFAULT NULL,
  `idKonta` int NOT NULL,
  PRIMARY KEY (`idKierowcy`,`idKonta`),
  KEY `fk_kierowcy_konta1_idx` (`idKonta`),
  CONSTRAINT `fk_kierowcy_konta1` FOREIGN KEY (`idKonta`) REFERENCES `konta` (`idKonta`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kierowcy`
--

LOCK TABLES `kierowcy` WRITE;
/*!40000 ALTER TABLE `kierowcy` DISABLE KEYS */;
INSERT INTO `kierowcy` VALUES (1,'default','kierowca','000000000',1,2),(2,'Kowalczyk','Piotr','123654321',1,4),(3,'Drózd','Jakub','123456789',1,5),(4,'Wójcik','Marek','123456789',1,6);
/*!40000 ALTER TABLE `kierowcy` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `klienci`
--

DROP TABLE IF EXISTS `klienci`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `klienci` (
  `idKlienci` int NOT NULL AUTO_INCREMENT,
  `nazwisko_klienta` varchar(45) DEFAULT NULL,
  `imie_klienta` varchar(45) DEFAULT NULL,
  `numer_tel_klienta` varchar(9) DEFAULT NULL,
  `weryfikacja` tinyint DEFAULT NULL,
  `czy_aktywne` tinyint NOT NULL,
  `idKonta` int NOT NULL,
  PRIMARY KEY (`idKlienci`,`idKonta`),
  KEY `fk_klienci_konta1_idx` (`idKonta`),
  CONSTRAINT `fk_klienci_konta1` FOREIGN KEY (`idKonta`) REFERENCES `konta` (`idKonta`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `klienci`
--

LOCK TABLES `klienci` WRITE;
/*!40000 ALTER TABLE `klienci` DISABLE KEYS */;
INSERT INTO `klienci` VALUES (1,'Kowalski','Jan','123456789',1,1,3),(2,'Kowalczyk','Piotr','123654321',1,0,4),(3,'Drózd','Jakub','123456789',1,0,5),(4,'Wójcik','Marek','123456789',1,0,6),(5,'Łuczak','Alicja','123456789',1,1,7),(6,'Nowak','Adam','123456789',1,1,8);
/*!40000 ALTER TABLE `klienci` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `konta`
--

DROP TABLE IF EXISTS `konta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `konta` (
  `idKonta` int NOT NULL AUTO_INCREMENT,
  `email_klienta` varchar(45) DEFAULT NULL,
  `login` varchar(45) DEFAULT NULL,
  `haslo` varchar(45) DEFAULT NULL,
  `rola` enum('Kierowca','Klient','Admin') DEFAULT NULL,
  PRIMARY KEY (`idKonta`),
  UNIQUE KEY `idKonta_UNIQUE` (`idKonta`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `konta`
--

LOCK TABLES `konta` WRITE;
/*!40000 ALTER TABLE `konta` DISABLE KEYS */;
INSERT INTO `konta` VALUES (1,'administrator@wp.pl','admin','admin','Admin'),(2,'kierowca@wp.pl','kierowca','kierowca','Kierowca'),(3,'jan@op.pl','klient1','klient1','Klient'),(4,'piotr@kow.pl','kierowca1','kierowca1','Kierowca'),(5,'jd@jd.jd','kierowca2','kierowca2','Kierowca'),(6,'mw@mw.mw','kierowca3','kierowca3','Kierowca'),(7,'al@al.al','klient2','klient2','Klient'),(8,'an@an.an','klient3','klient3','Klient');
/*!40000 ALTER TABLE `konta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `model_samochodu`
--

DROP TABLE IF EXISTS `model_samochodu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `model_samochodu` (
  `idModel` int NOT NULL AUTO_INCREMENT,
  `nazwa_model` varchar(45) DEFAULT NULL,
  `cena_za_km` double DEFAULT NULL,
  `zdj_model` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idModel`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `model_samochodu`
--

LOCK TABLES `model_samochodu` WRITE;
/*!40000 ALTER TABLE `model_samochodu` DISABLE KEYS */;
INSERT INTO `model_samochodu` VALUES (1,'Tir',2.49,'tir.png'),(2,'Towarowy1',5,'towar1.png'),(3,'GigaChad',420,'zigzak.png'),(4,'Towarowy2',5,'towar2.png'),(5,'Towarowy3',5,'towar3.png');
/*!40000 ALTER TABLE `model_samochodu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pensja`
--

DROP TABLE IF EXISTS `pensja`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pensja` (
  `idPensja` int NOT NULL AUTO_INCREMENT,
  `stawka_godz` double DEFAULT NULL,
  `miesiac` date DEFAULT NULL,
  `idKierowcy` int NOT NULL,
  PRIMARY KEY (`idPensja`,`idKierowcy`),
  KEY `fk_pensja_kierowcy1_idx` (`idKierowcy`),
  CONSTRAINT `fk_pensja_kierowcy1` FOREIGN KEY (`idKierowcy`) REFERENCES `kierowcy` (`idKierowcy`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pensja`
--

LOCK TABLES `pensja` WRITE;
/*!40000 ALTER TABLE `pensja` DISABLE KEYS */;
INSERT INTO `pensja` VALUES (1,20,'2022-01-01',2),(2,12,'2022-01-01',3),(3,20,'2022-06-01',4),(4,50,'2022-02-01',4),(5,30,'2022-02-01',3),(6,20,'2022-06-01',2),(7,20,'2022-05-01',2),(8,25,'2022-03-01',3),(9,35,'2022-05-01',4),(10,21,'2022-05-01',3),(11,27,'2022-06-01',3),(14,12,'2013-05-01',3),(15,40,'2013-06-01',4),(16,21,'2016-07-01',4),(17,25,'2005-06-01',4),(18,12,'2022-01-01',4);
/*!40000 ALTER TABLE `pensja` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pojazdy`
--

DROP TABLE IF EXISTS `pojazdy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pojazdy` (
  `idPojazdy` int NOT NULL AUTO_INCREMENT,
  `nr_rejestracji` varchar(7) DEFAULT NULL,
  `dostepnosc` tinyint DEFAULT NULL,
  `status_pojazdu` enum('Zajety','Zepsuty','Naprawa','Wolny') DEFAULT NULL,
  `idModel` int NOT NULL,
  PRIMARY KEY (`idPojazdy`,`idModel`),
  KEY `fk_pojazdy_typ1_idx` (`idModel`),
  CONSTRAINT `fk_pojazdy_model1` FOREIGN KEY (`idModel`) REFERENCES `model_samochodu` (`idModel`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pojazdy`
--

LOCK TABLES `pojazdy` WRITE;
/*!40000 ALTER TABLE `pojazdy` DISABLE KEYS */;
INSERT INTO `pojazdy` VALUES (1,'SK23456',1,'Wolny',1),(2,'SK23444',1,'Wolny',2),(3,'SK98746',1,'Wolny',4),(4,'SK09180',1,'Wolny',5),(5,'SK18903',1,'Wolny',3),(6,'SK14200',1,'Zajety',3),(7,'SK12345',1,'Zepsuty',1),(8,'SK37216',1,'Naprawa',2),(9,'SK21156',0,'Wolny',3),(10,'SK21376',1,'Wolny',4),(11,'SK21456',1,'Wolny',5);
/*!40000 ALTER TABLE `pojazdy` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `szczegoly_zamowienia`
--

DROP TABLE IF EXISTS `szczegoly_zamowienia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `szczegoly_zamowienia` (
  `idSzczegoly_zamowienia` int NOT NULL AUTO_INCREMENT,
  `poczatek_panstwo` varchar(45) DEFAULT NULL,
  `poczatek_miasto` varchar(45) DEFAULT NULL,
  `poczatek_ulica` varchar(45) DEFAULT NULL,
  `poczatek_nr_domu` varchar(45) DEFAULT NULL,
  `poczatek_kod_pocztowy` varchar(6) DEFAULT NULL,
  `koniec_panstwo` varchar(45) DEFAULT NULL,
  `koniec_miasto` varchar(45) DEFAULT NULL,
  `koniec_ulica` varchar(45) DEFAULT NULL,
  `koniec_nr_domu` varchar(45) DEFAULT NULL,
  `koniec_kod_pocztowy` varchar(6) DEFAULT NULL,
  `ilosc_km` double DEFAULT NULL,
  `cena` double DEFAULT NULL,
  PRIMARY KEY (`idSzczegoly_zamowienia`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `szczegoly_zamowienia`
--

LOCK TABLES `szczegoly_zamowienia` WRITE;
/*!40000 ALTER TABLE `szczegoly_zamowienia` DISABLE KEYS */;
INSERT INTO `szczegoly_zamowienia` VALUES (1,'Polska','Warszawa','Długa','12','20-300','Niemcy','Berlin','Prosta','15D','24-700',NULL,NULL),(2,'Polska','Jastrzębie-Zdrój','Prosta','9D','15-300','Polska','Warszawa','Długa','15','21-390',300,747.0000000000001),(3,'Polska','Bytom','Piłsudskiego','12D','41-902','Niemcy','Berlin','Długa','13','15-105',5000,2100000),(4,'Polska','Katowice','Polna','13','41-500','Polska','Jastrzębie-Zdrój','Długa','34D','34-900',NULL,NULL),(5,'Polska','Katowice','Polna','12','12-345','Polska','Bytom','Zabrzańska','12','12-345',NULL,NULL),(30,'Polska','Kielce','polna','3','41-902','USA','Las Vegas','69','4','12-345',NULL,NULL);
/*!40000 ALTER TABLE `szczegoly_zamowienia` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `zamowienia`
--

DROP TABLE IF EXISTS `zamowienia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `zamowienia` (
  `idZamowienia` int NOT NULL AUTO_INCREMENT,
  `data_zlozenia_zamowienia` datetime DEFAULT NULL,
  `zamowiona_data_realizacji` datetime DEFAULT NULL,
  `data_poczatku` datetime DEFAULT NULL,
  `data_konca` datetime DEFAULT NULL,
  `status_zamowienia` enum('Oczekuje','Zatwierdzony','Realizowany','Zakonczony','Anulowany') DEFAULT NULL,
  `idKierowcy` int NOT NULL,
  `idModel` int NOT NULL,
  `idKlienci` int NOT NULL,
  `idKonta` int NOT NULL,
  `idSzczegoly_zamowienia` int NOT NULL,
  `idAdministrator` int NOT NULL,
  `idPojazdy` int NOT NULL,
  `komentarz` varchar(200) DEFAULT NULL,
  `przewidywany_czas` int DEFAULT NULL,
  PRIMARY KEY (`idZamowienia`,`idKierowcy`,`idModel`,`idKlienci`,`idKonta`,`idSzczegoly_zamowienia`,`idAdministrator`,`idPojazdy`),
  KEY `fk_zamowienia_kierowcy1_idx` (`idKierowcy`),
  KEY `fk_zamowienia_klienci1_idx` (`idKlienci`,`idKonta`),
  KEY `fk_zamowienia_szczegoly_zamowienia1_idx` (`idSzczegoly_zamowienia`),
  KEY `fk_zamowienia_administrator1_idx` (`idAdministrator`),
  KEY `fk_zamowienia_model_samochodu1_idx` (`idModel`),
  KEY `fk_zamowienia_pojazdy1_idx` (`idPojazdy`),
  CONSTRAINT `fk_zamowienia_administrator1` FOREIGN KEY (`idAdministrator`) REFERENCES `administrator` (`idAdministrator`),
  CONSTRAINT `fk_zamowienia_kierowcy1` FOREIGN KEY (`idKierowcy`) REFERENCES `kierowcy` (`idKierowcy`),
  CONSTRAINT `fk_zamowienia_klienci1` FOREIGN KEY (`idKlienci`, `idKonta`) REFERENCES `klienci` (`idKlienci`, `idKonta`),
  CONSTRAINT `fk_zamowienia_model_samochodu1` FOREIGN KEY (`idModel`) REFERENCES `model_samochodu` (`idModel`),
  CONSTRAINT `fk_zamowienia_pojazdy1` FOREIGN KEY (`idPojazdy`) REFERENCES `pojazdy` (`idPojazdy`) ON DELETE RESTRICT,
  CONSTRAINT `fk_zamowienia_szczegoly_zamowienia1` FOREIGN KEY (`idSzczegoly_zamowienia`) REFERENCES `szczegoly_zamowienia` (`idSzczegoly_zamowienia`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zamowienia`
--

LOCK TABLES `zamowienia` WRITE;
/*!40000 ALTER TABLE `zamowienia` DISABLE KEYS */;
INSERT INTO `zamowienia` VALUES (2,'2022-05-23 20:40:46','2022-06-03 15:45:00','2022-05-23 20:47:52','2022-05-23 20:49:03','Zakonczony',3,1,5,7,1,1,1,NULL,5),(3,'2022-05-23 20:43:59','2022-06-01 15:15:00','2022-05-23 20:48:36','2022-05-23 20:49:03','Zakonczony',3,5,5,7,2,1,4,'zrealizowane poprawnie',5),(4,'2022-05-23 21:05:35','2022-06-01 14:30:00','2022-05-23 20:48:36','2022-05-23 20:49:03','Zakonczony',2,3,6,8,3,1,5,'brak opłaty',231),(5,'2022-06-06 21:40:32','2022-06-24 14:24:00','2022-05-23 20:48:36','2022-05-23 20:49:03','Zakonczony',2,3,1,3,4,1,5,'brak odpowiedzi klienta',32),(6,'2022-06-10 15:30:00','2022-06-15 15:15:00','2022-05-23 20:48:36','2022-06-23 20:49:03','Zakonczony',4,2,5,7,5,1,2,NULL,23),(29,'2022-06-27 17:54:06','2022-07-10 15:15:00',NULL,NULL,'Oczekuje',1,3,1,3,30,1,9,NULL,NULL);
/*!40000 ALTER TABLE `zamowienia` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-06-27 19:23:14
