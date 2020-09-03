-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               10.4.11-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             11.0.0.5919
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for table discord.users
CREATE TABLE IF NOT EXISTS `users` (
  `Id` bigint(20) unsigned NOT NULL DEFAULT 0,
  `Returner` tinyint(4) NOT NULL DEFAULT 0,
  `UniqueCommandModifier` tinyint(4) NOT NULL DEFAULT 0,
  `Name` text NOT NULL,
  `Username` text NOT NULL,
  `FavoriteCharacter` mediumtext NOT NULL DEFAULT '',
  `IsLewd` tinyint(4) NOT NULL DEFAULT 1,
  `MarriedUserId` bigint(20) unsigned NOT NULL DEFAULT 0,
  `MessagesSent` int(11) NOT NULL,
  `LoliWordUsages` int(11) NOT NULL,
  `AbbyNameUsages` int(11) NOT NULL,
  `CommandsSent` int(11) NOT NULL,
  `LonlinessRating` int(11) NOT NULL,
  `FavoriteChannel` varchar(50) NOT NULL DEFAULT '',
  `FavoritePing` varchar(50) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
