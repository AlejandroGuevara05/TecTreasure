CREATE DATABASE  IF NOT EXISTS `TecTreasureDB` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `TecTreasureDB`;
-- MySQL dump 10.13  Distrib 8.0.34, for macos13 (arm64)
--
-- Host: 127.0.0.1    Database: ciclo2
-- ------------------------------------------------------
-- Server version	8.0.34

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
-- Table structure for table `Admin`
--

DROP TABLE IF EXISTS `Admin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Admin` (
  `matricula` varchar(50) NOT NULL,
  `contrasena_admin` varchar(50) NOT NULL,
  PRIMARY KEY (`matricula`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Admin`
--

LOCK TABLES `Admin` WRITE;
/*!40000 ALTER TABLE `Admin` DISABLE KEYS */;
INSERT INTO `Admin` VALUES ('A00832930','equipoIOTjesus'),('A00834015','equipoIOTmiguel'),('A00834438','equipoIOT'),('A01284529','equipoIOTandres'),('A01570972','equipoIOTemiliano');
/*!40000 ALTER TABLE `Admin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `CiudadUsuario`
--

DROP TABLE IF EXISTS `CiudadUsuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `CiudadUsuario` (
  `id_tipo` int NOT NULL,
  `id_item` int NOT NULL,
  `id_usuario` int NOT NULL,
  PRIMARY KEY (`id_tipo`,`id_item`),
  KEY `Foreign_idTipoCity` (`id_tipo`),
  KEY `Foreign_idUsuarioInventario` (`id_item`,`id_usuario`),
  CONSTRAINT `Foreign_idTipoCity` FOREIGN KEY (`id_tipo`) REFERENCES `Item` (`id_tipo`),
  CONSTRAINT `Foreign_idUsuarioInventario` FOREIGN KEY (`id_item`, `id_usuario`) REFERENCES `UsuarioItem` (`id_item`, `id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CiudadUsuario`
--

LOCK TABLES `CiudadUsuario` WRITE;
/*!40000 ALTER TABLE `CiudadUsuario` DISABLE KEYS */;
/*!40000 ALTER TABLE `CiudadUsuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Compra`
--

DROP TABLE IF EXISTS `Compra`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Compra` (
  `id_compra` int NOT NULL AUTO_INCREMENT,
  `id_lootbox` int NOT NULL,
  `id_usuario` int NOT NULL,
  `id_tipoPago` int NOT NULL,
  `fecha_compra` date NOT NULL,
  `usado` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id_compra`),
  KEY `Foreign_lootbox` (`id_lootbox`),
  KEY `Foreign_usuario` (`id_usuario`),
  KEY `Foreign_pago` (`id_tipoPago`),
  CONSTRAINT `Foreign_lootbox` FOREIGN KEY (`id_lootbox`) REFERENCES `Lootbox` (`id_lootbox`),
  CONSTRAINT `Foreign_pago` FOREIGN KEY (`id_tipoPago`) REFERENCES `TipoPago` (`id_tipoPago`),
  CONSTRAINT `Foreign_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `Usuario` (`id_usuario`)
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Compra`
--

LOCK TABLES `Compra` WRITE;
/*!40000 ALTER TABLE `Compra` DISABLE KEYS */;
INSERT INTO `Compra` VALUES (2,1,11,1,'2023-11-22',1),(3,1,11,1,'2023-11-03',1),(4,1,13,1,'2022-11-30',0),(5,8,11,1,'2023-02-15',1),(6,2,11,1,'2023-11-26',1),(7,3,11,1,'2023-11-26',1),(8,4,11,1,'2023-11-26',1),(9,5,11,1,'2023-11-26',1),(10,6,11,1,'2023-11-26',1),(11,7,11,1,'2023-11-26',1),(12,1,11,1,'2023-11-28',1),(13,2,11,1,'2023-11-28',1),(14,3,11,1,'2023-11-28',1),(15,4,11,1,'2023-11-28',1),(16,5,11,1,'2023-11-28',1),(17,6,11,1,'2023-11-28',1),(18,7,11,1,'2023-11-28',1),(19,8,11,1,'2023-11-28',1),(20,1,11,1,'2023-11-28',1),(21,2,11,1,'2023-11-28',1),(22,2,11,1,'2023-11-28',1),(23,2,11,1,'2023-11-28',1),(24,1,11,1,'2023-11-28',1),(25,1,11,1,'2023-11-28',1),(26,2,11,1,'2023-11-28',1),(27,3,11,1,'2023-11-28',1),(28,4,11,1,'2023-11-28',1),(29,5,11,1,'2023-11-28',1),(30,1,11,1,'2023-11-28',1),(31,1,11,1,'2023-11-28',1),(32,1,11,1,'2023-11-28',1),(33,1,11,1,'2023-11-28',1),(34,1,11,1,'2023-11-28',1),(35,3,11,1,'2023-11-28',1),(36,6,11,1,'2023-11-28',1),(37,2,11,1,'2023-11-28',1),(38,3,11,1,'2023-11-28',1),(39,1,11,1,'2023-11-28',1),(40,1,11,1,'2023-11-28',1),(41,2,11,1,'2023-11-28',1),(42,3,11,1,'2023-11-28',1),(43,3,11,1,'2023-11-28',1),(44,3,11,1,'2023-11-28',1),(45,1,11,1,'2023-11-28',1),(46,1,11,1,'2023-11-28',1),(47,2,11,1,'2023-11-28',1),(48,1,11,1,'2023-11-28',1),(49,3,11,1,'2023-11-28',1),(50,2,18,1,'2023-11-28',1),(51,7,18,1,'2023-11-28',0),(52,8,18,1,'2023-11-28',1),(53,8,18,1,'2023-11-28',0),(54,1,11,1,'2023-11-29',1),(55,1,11,1,'2023-11-29',1),(56,1,23,1,'2023-11-30',1),(57,1,23,1,'2023-11-30',1),(58,3,23,1,'2023-11-30',1),(59,8,23,1,'2023-11-30',0),(60,8,23,1,'2023-11-30',0),(61,2,11,1,'2023-11-30',1),(62,2,11,1,'2023-11-30',1),(63,1,11,1,'2023-11-30',1),(64,1,11,1,'2023-11-30',1),(65,2,11,1,'2023-11-30',1),(66,3,11,1,'2023-11-30',1),(67,1,11,1,'2023-11-30',1),(68,1,11,1,'2023-11-30',0),(69,2,11,1,'2023-11-30',0),(70,3,11,1,'2023-11-30',0);
/*!40000 ALTER TABLE `Compra` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Item`
--

DROP TABLE IF EXISTS `Item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Item` (
  `id_item` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `id_tipo` int NOT NULL,
  `descripcion` varchar(255) NOT NULL,
  `id_lootbox` int NOT NULL,
  PRIMARY KEY (`id_item`),
  KEY `Foreign_lootBoxx` (`id_lootbox`),
  KEY `idx_id_tipo` (`id_tipo`),
  CONSTRAINT `fk` FOREIGN KEY (`id_tipo`) REFERENCES `ItemTipo` (`id_tipo`),
  CONSTRAINT `Foreign_lootBoxx` FOREIGN KEY (`id_lootbox`) REFERENCES `Lootbox` (`id_lootbox`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Item`
--

LOCK TABLES `Item` WRITE;
/*!40000 ALTER TABLE `Item` DISABLE KEYS */;
INSERT INTO `Item` VALUES (2,'Nina',1,'Skin de personaje de la legendaria Nina',1),(3,'Mesa de madera',2,'Mesa de madera para colocar en la casa',1),(4,'20% en Mi Sueño',3,'Descuento de 20% en la compra de un boleto para el sorteo Mi Sueño',1),(5,'Mi Sueño',4,'¡Boleto para participar en el sorteo Mi Sueño de SorteosTec!',1),(6,'Hechicera',1,'Skin de hechicera misteriosa',2),(7,'Sillón Individual Amarillo',2,'Sillón individual color amarillo para colocar en la casa',2),(8,'20% en Aventurat',3,'Descuento de 20% en la compra de un boleto para el Sorteo Aventurat',2),(9,'Aventurat',4,'¡Boleto para participar en el sorteo Aventurat de SorteosTec!',2),(10,'Caballero',1,'Skin de personaje de caballero medieval',3),(11,'Piso de Madera',2,'Piso de madera para colocar en la casa',3),(12,'20% en Educativo',3,'Descuento de 20% en la compra de un boleto para el Sorteo Educativo',3),(13,'Educativo',4,'¡Boleto para participar en el sorteo Educativo de SorteosTec!',3),(14,'Bandido',1,'Skin de personaje de bandido misterioso',4),(15,'Pared de Ladrillo',2,'Pared de ladrillo para colocar en la casa',4),(16,'20% en Casa Hábitat',3,'Descuento de 20% en la compra de un boleto para el Sorteo Casa Hábitat',4),(17,'Casa Hábitat',4,'¡Boleto para participar en el sorteo Casa Hábitat de SorteosTec!',4),(18,'Ninja',1,'Skin de personaje de ninja',5),(19,'Lámpara',2,'Lámpara sobre mesa para colocar en la casa',5),(20,'25% en Efec+ivo Plus',3,'Descuento de 25% en la compra de un boleto para el Sorteo Efec+ivo Plus',5),(21,'Efec+ivo Plus',4,'¡Boleto para participar en el sorteo Efec+ivo Plus de SorteosTec!',5),(22,'Caballero de Fuego',1,'Skin de personaje de caballero de fuego',6),(23,'Sillón Doble',2,'Sillón doble bicolor para colocar en la casa',6),(24,'25% en Dinero de X Vida',3,'Descuento de 25% en la compra de un boleto para el Sorteo Dinero de X Vida',6),(25,'Dinero de X Vida',4,'¡Boleto para participar en el sorteo Dinero de X Vida de SorteosTec!',6),(26,'Caballero Romano',1,'Skin de personaje de caballero romano',7),(27,'Puerta Lujosa',2,'Puerta lujosa de madera para colocar en la casa',7),(28,'25% en Lo Quiero',3,'Descuento de 25% en la compra de un boleto para el Sorteo Lo Quiero',7),(29,'Lo Quiero',4,'¡Boleto para participar en el sorteo Lo Quiero de SorteosTec!',7),(30,'Caballero Oscuro',1,'Skin de personaje de caballero oscuro legendario',8),(31,'TV',2,'TV para colocar en la casa',8),(32,'25% en Tradicional',3,'Descuento de 25% en la compra de un boleto para el Sorteo Tradicional',8),(33,'Tradicional',4,'¡Boleto para participar en el sorteo Tradicional de SorteosTec!',8);
/*!40000 ALTER TABLE `Item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ItemTipo`
--

DROP TABLE IF EXISTS `ItemTipo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ItemTipo` (
  `id_tipo` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  PRIMARY KEY (`id_tipo`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ItemTipo`
--

LOCK TABLES `ItemTipo` WRITE;
/*!40000 ALTER TABLE `ItemTipo` DISABLE KEYS */;
INSERT INTO `ItemTipo` VALUES (1,'Skin'),(2,'Objeto de Casa'),(3,'Descuento'),(4,'Boleto');
/*!40000 ALTER TABLE `ItemTipo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Lootbox`
--

DROP TABLE IF EXISTS `Lootbox`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Lootbox` (
  `id_lootbox` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `precio_moneditas` int NOT NULL,
  `precio_gemas` int NOT NULL,
  `tipo` varchar(30) NOT NULL,
  `status` tinyint(1) NOT NULL,
  PRIMARY KEY (`id_lootbox`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Lootbox`
--

LOCK TABLES `Lootbox` WRITE;
/*!40000 ALTER TABLE `Lootbox` DISABLE KEYS */;
INSERT INTO `Lootbox` VALUES (1,'Mi Sueño',3000,25,'Lootbox',1),(2,'Aventurat',3000,25,'Lootbox',1),(3,'Educativo',3000,25,'Lootbox',1),(4,'Casa Hábitat',3000,25,'Lootbox',1),(5,'Efec+ivo Plus',4000,30,'Lootbox',1),(6,'Dinero de X Vida',4000,30,'Lootbox',1),(7,'Lo Quiero',4000,30,'Lootbox',1),(8,'Tradicional',4000,30,'Lootbox',1);
/*!40000 ALTER TABLE `Lootbox` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Resena`
--

DROP TABLE IF EXISTS `Resena`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Resena` (
  `id_resena` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `fecha_resena` date NOT NULL,
  `calificacion` int NOT NULL,
  `texto` varchar(255) NOT NULL,
  PRIMARY KEY (`id_resena`),
  KEY `Foreign_idUsuario` (`id_usuario`),
  CONSTRAINT `Foreign_idUsuario` FOREIGN KEY (`id_usuario`) REFERENCES `Usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Resena`
--

LOCK TABLES `Resena` WRITE;
/*!40000 ALTER TABLE `Resena` DISABLE KEYS */;
/*!40000 ALTER TABLE `Resena` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `TipoPago`
--

DROP TABLE IF EXISTS `TipoPago`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TipoPago` (
  `id_tipoPago` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `descripcion` varchar(255) NOT NULL,
  PRIMARY KEY (`id_tipoPago`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `TipoPago`
--

LOCK TABLES `TipoPago` WRITE;
/*!40000 ALTER TABLE `TipoPago` DISABLE KEYS */;
INSERT INTO `TipoPago` VALUES (1,'Tarjeta de crédito','Pago mediante el uso de tarjeta de crédito.');
/*!40000 ALTER TABLE `TipoPago` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Usuario`
--

DROP TABLE IF EXISTS `Usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Usuario` (
  `id_usuario` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `sexo` varchar(100) DEFAULT NULL,
  `edad` int NOT NULL,
  `estado` varchar(100) NOT NULL,
  `correo` varchar(50) NOT NULL,
  `maxpuntuacion` int DEFAULT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `contrasena_web` varchar(50) DEFAULT NULL,
  `activo` int NOT NULL,
  PRIMARY KEY (`id_usuario`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Usuario`
--

LOCK TABLES `Usuario` WRITE;
/*!40000 ALTER TABLE `Usuario` DISABLE KEYS */;
INSERT INTO `Usuario` VALUES (1,'Juan Pérez','Masculino',28,'Jalisco','juan.perez@example.com',85,NULL,'Qwerty123!',0),(2,'Luisa Gómez','Femenino',34,'Ciudad de México','luisa.gomez@example.com',92,NULL,'SecurePass987',0),(3,'Carlos Sánchez','Masculino',41,'Nuevo León','carlos.sanchez@example.com',75,NULL,'Passw0rd456',0),(4,'María Fernández','Femenino',22,'Jalisco','maria.fernandez@example.com',88,NULL,'BlueSky*789',0),(5,'Ana Méndez','Femenino',19,'Nuevo León','ana.mendez@example.com',95,NULL,'StarWars#42',0),(6,'Roberto Díaz','Masculino',30,'Ciudad de México','roberto.diaz@example.com',64,NULL,'Rainbow$567',0),(7,'Sofía Castillo','Femenino',26,'Ciudad de México','sofia.castillo@example.com',82,NULL,'CoffeeLover!99',0),(8,'Oscar Jiménez','Masculino',38,'Jalisco','oscar.jimenez@example.com',77,NULL,'CodingIsFun%2022',0),(9,'Lucía Hernández','Femenino',31,'Nuevo León','lucia.hernandez@example.com',69,NULL,'OceanWave*888',0),(10,'Raúl González','Masculino',29,'Jalisco','raul.gonzalez@example.com',90,NULL,'Holiwis',0),(11,'Alejandro Guevara Olivares','Masculino',20,'Veracruz','A00834438@tec.mx',NULL,'2297734483','sergio05_',0),(12,'Mike','Masculino',20,'Tamaulipas','A00894408@tec.mx',NULL,'7777734483','eldenRingLover',0),(13,'Jesús Galaz','Masculino',30,'Coahuila','elyisus@tec.mx',NULL,'2222734483','anapao02',0),(14,'Pedro Parker','Masculino',34,'Ciudad de México','pedri@gmail.com',NULL,'8137734483','spiderman',0),(15,'John Michael','Masculino',32,'Guerrero','john@hotmail.com',NULL,'3382203943','123',0),(16,'Chloe Price','Femenino',19,'Jalisco','punk@gmail.com',NULL,'4478830192','blue',0),(17,'Max Caulfield','Femenino',18,'Baja California','maxine@gmail.com',NULL,'8122323452','12345678',0),(18,'Joel Miller','Otro',44,'Puebla','joel@gmail.com',NULL,'2298834438','12345678Ellie',0),(19,'Francisco Argento','Masculino',45,'Sonora','fran@tec.mx',NULL,'2293384473','12345678fran',0),(20,'Shrek Pantano','Masculino',35,'Aguascalientes','shrek@gmail.com',NULL,'3342273354','12345678Fiona',0),(21,'emiliano salinas','Masculino',23,'Sinaloa','emiedubawediuawd@tec.mx',NULL,'8181818181','12345678910',0),(22,'Handsome Jack','Masculino',30,'Guanajuato','jack@outlook.com',NULL,'2293384493','12345678HEY',0),(23,'Alejandro Olivares','Masculino',20,'Campeche','hola@gmail.com',NULL,'2293342283','12345678Ki',0);
/*!40000 ALTER TABLE `Usuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UsuarioItem`
--

DROP TABLE IF EXISTS `UsuarioItem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UsuarioItem` (
  `id_usuario` int NOT NULL,
  `id_item` int NOT NULL,
  `cantidad` int NOT NULL,
  `coordenada_x` decimal(10,0) DEFAULT NULL,
  `coordenada_y` decimal(10,0) DEFAULT NULL,
  PRIMARY KEY (`id_usuario`,`id_item`),
  KEY `Foreign_idUser` (`id_usuario`),
  KEY `Foreign_idItem` (`id_item`),
  CONSTRAINT `Foreign_idItem` FOREIGN KEY (`id_item`) REFERENCES `Item` (`id_item`),
  CONSTRAINT `Foreign_idUser` FOREIGN KEY (`id_usuario`) REFERENCES `Usuario` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UsuarioItem`
--

LOCK TABLES `UsuarioItem` WRITE;
/*!40000 ALTER TABLE `UsuarioItem` DISABLE KEYS */;
INSERT INTO `UsuarioItem` VALUES (11,2,12,0,0),(11,3,17,0,0),(11,4,2,0,0),(11,6,4,0,0),(11,7,5,0,0),(11,8,2,0,0),(11,9,2,0,0),(11,10,2,0,0),(11,11,4,0,0),(11,12,5,0,0),(11,15,2,0,0),(11,16,2,0,0),(11,18,1,0,0),(11,19,3,0,0),(11,22,2,0,0),(11,23,2,0,0),(11,26,1,0,0),(11,27,2,0,0),(11,31,3,0,0),(11,32,2,0,0),(18,7,1,0,0),(18,32,1,0,0),(23,2,1,0,0),(23,3,1,0,0),(23,11,1,0,0);
/*!40000 ALTER TABLE `UsuarioItem` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-11-30 12:09:50
