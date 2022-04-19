-- phpMyAdmin SQL Dump
-- version 4.9.7
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : mar. 19 avr. 2022 à 20:01
-- Version du serveur :  5.7.36
-- Version de PHP : 7.4.26

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `tentclicker`
--

-- --------------------------------------------------------

--
-- Structure de la table `decoration`
--

DROP TABLE IF EXISTS `decoration`;
CREATE TABLE IF NOT EXISTS `decoration` (
  `id_game` varchar(6) NOT NULL COMMENT 'Identifiant de la sauvegarde associée',
  `type` tinyint(3) UNSIGNED NOT NULL COMMENT 'Type de décoration / arbre',
  `row` tinyint(3) UNSIGNED NOT NULL COMMENT 'Numéro de ligne',
  `col` tinyint(3) UNSIGNED NOT NULL COMMENT 'Numéro de colonne',
  KEY `id_game_fk` (`id_game`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Table représentant une décoration, associée à une sauvegarde';

-- --------------------------------------------------------

--
-- Structure de la table `game`
--

DROP TABLE IF EXISTS `game`;
CREATE TABLE IF NOT EXISTS `game` (
  `id` varchar(6) NOT NULL COMMENT 'Identifiant unique (6 caractères)',
  `score` int(10) UNSIGNED NOT NULL COMMENT 'Nombre de ressources',
  `click_level` tinyint(3) UNSIGNED NOT NULL COMMENT 'Niveau de l''upgrade click',
  `autogather_level` tinyint(3) UNSIGNED NOT NULL COMMENT 'Niveau de l''upgrade autogather',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Table principale d''une sauvegarde';

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `decoration`
--
ALTER TABLE `decoration`
  ADD CONSTRAINT `id_game_fk` FOREIGN KEY (`id_game`) REFERENCES `game` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
