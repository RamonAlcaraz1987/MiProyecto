-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 29-07-2025 a las 01:07:30
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `pokemoncartas`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `carta`
--

CREATE TABLE `carta` (
  `IdCarta` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `IdTipo1` int(11) DEFAULT NULL,
  `Imagen` varchar(200) DEFAULT NULL,
  `ValorEstimado` int(11) NOT NULL,
  `IdCategoria` int(11) NOT NULL,
  `Estado` tinyint(4) NOT NULL,
  `IdTipo2` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `carta`
--

INSERT INTO `carta` (`IdCarta`, `Nombre`, `IdTipo1`, `Imagen`, `ValorEstimado`, `IdCategoria`, `Estado`, `IdTipo2`) VALUES
(2, 'Bulbasaur', 5, '/CartasArte/carta_2.png', 1000, 1, 1, 15),
(3, 'Ivysaur', 5, '/CartasArte/carta_3.png', 1000, 1, 1, 15),
(4, 'Venasaur', 5, '/CartasArte/carta_4.png', 1000, 1, 1, 15),
(5, 'Charmander', 10, '/CartasArte/carta_5.png', 1000, 1, 1, 0),
(6, 'Charmeleon', 10, '/CartasArte/carta_6.png', 1000, 1, 1, 0),
(7, 'Charizard', 10, '/CartasArte/carta_7.png', 5000, 2, 1, 7),
(8, 'Squirtle', 3, '/CartasArte/carta_8.png', 1000, 1, 1, 0),
(9, 'Wartortle', 3, '/CartasArte/carta_9.png', 1000, 1, 1, 0),
(10, 'Blastoise', 3, '/CartasArte/carta_10.png', 5000, 2, 1, 0),
(11, 'Caterpie', 14, '/CartasArte/carta_11.png', 1000, 1, 1, 0),
(12, 'Metapod', 14, '/CartasArte/carta_12.png', 1000, 1, 1, 0),
(13, 'Butterfree', 14, '/CartasArte/carta_13.png', 1000, 1, 1, 7),
(14, 'Weedle', 14, '/CartasArte/carta_14.png', 1000, 1, 1, 15),
(15, 'Kakuna', 14, '/CartasArte/carta_15.png', 1000, 1, 1, 15),
(16, 'Beedrill', 14, '/CartasArte/carta_16.png', 1000, 1, 1, 15),
(17, 'Pidgey', 1, '/CartasArte/carta_17.png', 1000, 1, 1, 7),
(18, 'Pidgeotto', 1, '/CartasArte/carta_18.png', 1000, 1, 1, 7),
(19, 'Pidgeot', 1, '/CartasArte/carta_19.png', 1000, 1, 1, 7),
(20, 'Rattata', 1, '/CartasArte/carta_20.png', 1000, 1, 1, 0),
(21, 'Raticate', 1, '/CartasArte/carta_21.png', 1000, 1, 1, 0),
(22, 'Spearow', 1, '/CartasArte/carta_22.png', 1000, 1, 1, 7),
(23, 'Fearow', 1, '/CartasArte/carta_23.png', 1000, 1, 1, 7),
(24, 'Ekans', 15, '/CartasArte/carta_24.png', 1000, 1, 1, 0),
(25, 'Arbok', 15, '/CartasArte/carta_25.png', 1000, 1, 1, 0),
(26, 'Pikachu', 4, '/CartasArte/carta_26.png', 10000, 2, 1, 0),
(27, 'Raichu', 4, '/CartasArte/carta_27.png', 1000, 1, 1, 0),
(28, 'Sandshrew', 6, '/CartasArte/carta_28.png', 1000, 1, 1, 0),
(29, 'Sandslash', 6, '/CartasArte/carta_29.png', 1000, 1, 1, 6),
(30, 'NidoranF', 15, '/CartasArte/carta_30.png', 1000, 1, 1, 0),
(31, 'Nidorina', 15, '/CartasArte/carta_31.png', 1000, 1, 1, 0),
(32, 'Nidoqueen', 6, '/CartasArte/carta_32.png', 1000, 1, 1, 15),
(33, 'NidoranM', 15, '/CartasArte/carta_33.png', 1000, 1, 1, 0),
(34, 'Nidorino', 15, '/CartasArte/carta_34.png', 1000, 1, 1, 0),
(35, 'NidoKing', 6, '/CartasArte/carta_35.png', 1000, 1, 1, 15),
(36, 'Clefairy', 1, '/CartasArte/carta_36.png', 1000, 1, 1, 0),
(37, 'Clefable', 1, '/CartasArte/carta_37.png', 1000, 1, 1, 0),
(38, 'Vulpix', 10, '/CartasArte/carta_38.png', 1000, 1, 1, 0),
(39, 'Ninetales', 10, '/CartasArte/carta_39.png', 1000, 1, 1, 0),
(40, 'Jigglypuff', 1, '/CartasArte/carta_40.png', 1000, 1, 1, 0),
(41, 'Wigglytuff', 1, '/CartasArte/carta_41.png', 1000, 1, 1, 0),
(42, 'Zubat', 15, '/CartasArte/carta_42.png', 1000, 1, 1, 7),
(43, 'Golbat', 15, '/CartasArte/carta_43.png', 1000, 1, 1, 0),
(44, 'Oddish', 5, '/CartasArte/carta_44.png', 1000, 1, 1, 15),
(45, 'Gloom', 5, '/CartasArte/carta_45.png', 1000, 1, 1, 15),
(46, 'Vileplume', 5, '/CartasArte/carta_46.png', 1000, 1, 1, 15),
(47, 'Paras', 14, '/CartasArte/carta_47.png', 1000, 1, 1, 5),
(48, 'Parasect', 14, '/CartasArte/carta_48.png', 1000, 1, 1, 5),
(49, 'Venonat', 14, '/CartasArte/carta_49.png', 1000, 1, 1, 15),
(50, 'Venomoth', 14, '/CartasArte/carta_50.png', 1000, 1, 1, 15),
(51, 'Diglett', 6, '/CartasArte/carta_51.png', 1000, 1, 1, 0),
(52, 'Dugtrio', 6, '/CartasArte/carta_52.png', 1000, 1, 1, 0),
(53, 'Meowth', 1, '/CartasArte/carta_53.png', 1000, 1, 1, 0),
(54, 'Persian', 1, '/CartasArte/carta_54.png', 1000, 1, 1, 0),
(55, 'Golduck', 3, '/CartasArte/carta_55.png', 1000, 1, 1, 0),
(56, 'Psyduck', 3, '/CartasArte/carta_56.png', 1000, 1, 1, 0),
(57, 'Mankey', 8, '/CartasArte/carta_57.png', 1000, 1, 1, 0),
(58, 'Primeape', 8, '/CartasArte/carta_58.png', 1000, 1, 1, 0),
(59, 'Growlithe', 10, '/CartasArte/carta_59.png', 1000, 1, 1, 0),
(60, 'Arcanine', 10, '/CartasArte/carta_60.png', 1000, 1, 1, 0),
(61, 'Poliwag', 3, '/CartasArte/carta_61.png', 1000, 1, 1, 0),
(62, 'Poliwhirl', 3, '/CartasArte/carta_62.png', 1000, 1, 1, 0),
(63, 'Poliwrath', 3, '/CartasArte/carta_63.png', 1000, 1, 1, 8),
(64, 'Abra', 2, '/CartasArte/carta_64.png', 1000, 1, 1, 0),
(65, 'Kadabra', 2, '/CartasArte/carta_65.png', 1000, 1, 1, 0),
(66, 'Alakazam', 2, '/CartasArte/carta_66.png', 1000, 1, 1, 0),
(67, 'Machop', 8, '/CartasArte/carta_67.png', 1000, 1, 1, 0),
(68, 'Machoke', 8, '/CartasArte/carta_68.png', 1000, 1, 1, 0),
(69, 'Machamp', 8, '/CartasArte/carta_69.png', 1000, 1, 1, 0),
(70, 'Bellsprout', 5, '/CartasArte/carta_70.png', 1000, 1, 1, 15),
(71, 'Weepinbell', 5, '/CartasArte/carta_71.png', 1000, 1, 1, 15),
(72, 'Victreebell', 5, '/CartasArte/carta_72.png', 1000, 1, 1, 15),
(73, 'Tentacool', 5, '/CartasArte/carta_73.png', 1000, 1, 1, 15),
(74, 'Tentacruel', 3, '/CartasArte/carta_74.png', 1000, 1, 1, 15),
(75, 'Geodude', 12, '/CartasArte/carta_75.png', 1000, 1, 1, 6),
(76, 'Graveler', 12, '/CartasArte/carta_76.png', 1000, 1, 1, 6),
(77, 'Golem', 12, '/CartasArte/carta_77.png', 1000, 1, 1, 6),
(78, 'Ponyta', 10, '/CartasArte/carta_78.png', 1000, 1, 1, 0),
(79, 'Rapidash', 10, '/CartasArte/carta_79.png', 1000, 1, 1, 0),
(80, 'Slowpoke', 3, '/CartasArte/carta_80.png', 1000, 1, 1, 0),
(81, 'Slowbro', 3, '/CartasArte/carta_81.png', 1000, 1, 1, 0),
(82, 'Magnemite', 4, '/CartasArte/carta_82.png', 1000, 1, 1, 0),
(83, 'Magneton', 4, '/CartasArte/carta_83.png', 1000, 1, 1, 0),
(84, 'Farfetchd', 1, '/CartasArte/carta_84.png', 1000, 1, 1, 7),
(85, 'Doduo', 1, '/CartasArte/carta_85.png', 1000, 1, 1, 7),
(86, 'Dodrio', 1, '/CartasArte/carta_86.png', 1000, 1, 1, 7),
(87, 'Seel', 3, '/CartasArte/carta_87.png', 1000, 1, 1, 0),
(88, 'Dewgong', 3, '/CartasArte/carta_88.png', 1000, 1, 1, 13),
(89, 'Grimer', 15, '/CartasArte/carta_89.png', 1000, 1, 1, 0),
(90, 'Muk', 15, '/CartasArte/carta_90.png', 1000, 1, 1, 0),
(91, 'Shellder', 3, '/CartasArte/carta_91.png', 1000, 1, 1, 0),
(92, 'Cloyster', 3, '/CartasArte/carta_92.png', 1000, 1, 1, 13),
(93, 'Gastly', 9, '/CartasArte/carta_93.png', 1000, 1, 1, 15),
(94, 'Haunter', 9, '/CartasArte/carta_94.png', 1000, 1, 1, 15),
(95, 'Gengar', 9, '/CartasArte/carta_95.png', 1000, 1, 1, 15),
(96, 'Onix', 12, '/CartasArte/carta_96.png', 1000, 1, 1, 6),
(97, 'Drowzee', 2, '/CartasArte/carta_97.png', 1000, 1, 1, 0),
(98, 'Hypno', 2, '/CartasArte/carta_98.png', 1000, 1, 1, 0),
(99, 'Krabby', 3, '/CartasArte/carta_99.png', 1000, 1, 1, 0),
(100, 'Kingler', 3, '/CartasArte/carta_100.png', 1000, 1, 1, 0),
(101, 'Voltorb', 4, '/CartasArte/carta_101.png', 1000, 1, 1, 0),
(102, 'Electrode', 4, '/CartasArte/carta_102.png', 1000, 1, 1, 0),
(103, 'Exeggcute', 5, '/CartasArte/carta_103.png', 1000, 1, 1, 2),
(104, 'Exeggutor', 5, '/CartasArte/carta_104.png', 1000, 1, 1, 2),
(105, 'Cubone', 6, '/CartasArte/carta_105.png', 1000, 1, 1, 0),
(106, 'Marowak', 6, '/CartasArte/carta_106.png', 1000, 1, 1, 0),
(107, 'Hitmonlee', 8, '/CartasArte/carta_107.png', 1000, 1, 1, 0),
(108, 'Hitmonchan', 8, '/CartasArte/carta_108.png', 1000, 1, 1, 0),
(109, 'Lickitung', 1, '/CartasArte/carta_109.png', 1000, 1, 1, 0),
(110, 'Koffing', 15, '/CartasArte/carta_110.png', 1000, 1, 1, 0),
(111, 'Weezing', 15, '/CartasArte/carta_111.png', 1000, 1, 1, 0),
(112, 'Rhyhorn', 6, '/CartasArte/carta_112.png', 1000, 1, 1, 12),
(113, 'Rhydon', 6, '/CartasArte/carta_113.png', 1000, 1, 1, 12),
(114, 'Chansey', 1, '/CartasArte/carta_114.png', 1000, 1, 1, 0),
(115, 'Tangela', 5, '/CartasArte/carta_115.png', 1000, 1, 1, 0),
(116, 'Kangaskhan', 1, '/CartasArte/carta_116.png', 1000, 1, 1, 0),
(117, 'Horsea', 3, '/CartasArte/carta_117.png', 1000, 1, 1, 0),
(118, 'Seadra', 3, '/CartasArte/carta_118.png', 1000, 1, 1, 0),
(119, 'Goldeen', 3, '/CartasArte/carta_119.png', 1000, 1, 1, 0),
(120, 'Seaking', 3, '/CartasArte/carta_120.png', 1000, 1, 1, 0),
(121, 'Staryu', 3, '/CartasArte/carta_121.png', 1000, 1, 1, 0),
(122, 'Starmie', 3, '/CartasArte/carta_122.png', 1000, 1, 1, 2),
(123, 'MrMime', 2, '/CartasArte/carta_123.png', 1000, 1, 1, 0),
(124, 'Scyther', 14, '/CartasArte/carta_124.png', 1000, 1, 1, 0),
(125, 'Jynx', 13, '/CartasArte/carta_125.png', 1000, 1, 1, 2),
(126, 'Electabuzz', 4, '/CartasArte/carta_126.png', 1000, 1, 1, 0),
(127, 'Magmar', 10, '/CartasArte/carta_127.png', 1000, 1, 1, 0),
(128, 'Pinsir', 14, '/CartasArte/carta_128.png', 1000, 1, 1, 0),
(129, 'Tauros', 14, '/CartasArte/carta_129.png', 1000, 1, 1, 0),
(130, 'Magikarp', 3, '/CartasArte/carta_130.png', 1000, 1, 1, 0),
(131, 'Gyarados', 3, '/CartasArte/carta_131.png', 1000, 1, 1, 7),
(132, 'Lapras', 3, '/CartasArte/carta_132.png', 1000, 1, 1, 13),
(133, 'Ditto', 1, '/CartasArte/carta_133.png', 1000, 1, 1, 0),
(134, 'Eevee', 1, '/CartasArte/carta_134.png', 1000, 1, 1, 0),
(135, 'Vaporeon', 3, '/CartasArte/carta_135.png', 1000, 1, 1, 0),
(136, 'Jolteon', 4, '/CartasArte/carta_136.png', 1000, 1, 1, 0),
(137, 'Flareon', 10, '/CartasArte/carta_137.png', 1000, 1, 1, 0),
(138, 'Porygon', 1, '/CartasArte/carta_138.png', 1000, 1, 1, 0),
(139, 'Omanyte', 3, '/CartasArte/carta_139.png', 1000, 1, 1, 12),
(140, 'Omastar', 3, '/CartasArte/carta_140.png', 1000, 1, 1, 12),
(141, 'Kabuto', 12, '/CartasArte/carta_141.png', 1000, 1, 1, 3),
(142, 'Kabutops', 12, '/CartasArte/carta_142.png', 1000, 1, 1, 3),
(143, 'Aerodactyl', 12, '/CartasArte/carta_143.png', 1000, 1, 1, 7),
(144, 'Snorlax', 1, '/CartasArte/carta_144.png', 1000, 1, 1, 0),
(145, 'Articuno', 13, '/CartasArte/carta_145.png', 5000, 2, 1, 7),
(146, 'Zapdos', 4, '/CartasArte/carta_146.png', 1000, 2, 1, 7),
(147, 'Moltres', 10, '/CartasArte/carta_147.png', 5000, 2, 1, 7),
(148, 'Dratini', 11, '/CartasArte/carta_148.png', 1000, 1, 1, 0),
(149, 'Dragonair', 11, '/CartasArte/carta_149.png', 1000, 1, 1, 0),
(150, 'Dragonite', 11, '/CartasArte/carta_150.png', 5000, 2, 1, 7),
(151, 'Mewtwo', 2, '/CartasArte/carta_151.png', 40000, 4, 1, 0),
(152, 'Mew', 2, '/CartasArte/carta_152.png', 40000, 4, 1, 0),
(153, 'Pikachu1', 4, '/CartasArte/carta_153.png', 40000, 4, 1, 0),
(154, 'Charmander1', 10, '/CartasArte/carta_154.png', 20000, 3, 1, 0),
(155, 'Charmeleon1', 10, '/CartasArte/carta_155.png', 20000, 3, 1, 0),
(156, 'Charizard1', 10, '/CartasArte/carta_156.png', 20000, 3, 1, 0),
(157, 'Bulbasaur1', 5, '/CartasArte/carta_157.png', 20000, 3, 1, 15),
(158, 'ivysaur1', 5, '/CartasArte/carta_158.png', 20000, 3, 1, 15),
(159, 'Venasaur1', 5, '/CartasArte/carta_159.png', 20000, 3, 1, 15),
(160, 'Squirtle1', 3, '/CartasArte/carta_160.png', 20000, 3, 1, 0),
(161, 'Wartortle1', 3, '/CartasArte/carta_161.png', 20000, 3, 1, 0),
(162, 'Blastoise1', 3, '/CartasArte/carta_162.png', 20000, 3, 1, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `categoria`
--

CREATE TABLE `categoria` (
  `IdCategoria` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `categoria`
--

INSERT INTO `categoria` (`IdCategoria`, `Nombre`) VALUES
(1, 'Comun'),
(2, 'Rara'),
(3, 'Epica'),
(4, 'Legendaria');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `coleccion`
--

CREATE TABLE `coleccion` (
  `IdColeccion` int(11) NOT NULL,
  `IdUsuario` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `EsPublica` tinyint(1) NOT NULL DEFAULT 1,
  `Estado` tinyint(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `coleccion`
--

INSERT INTO `coleccion` (`IdColeccion`, `IdUsuario`, `Nombre`, `EsPublica`, `Estado`) VALUES
(3, 1, 'Mi coleccion Ramon Alcaraz', 1, 1),
(5, 3, 'Mi coleccion Milton Garcia', 1, 1),
(6, 2, 'Mi coleccion Claudio Garro', 0, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `coleccion_carta`
--

CREATE TABLE `coleccion_carta` (
  `IdColeccion` int(11) NOT NULL,
  `IdCarta` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `coleccion_carta`
--

INSERT INTO `coleccion_carta` (`IdColeccion`, `IdCarta`, `Cantidad`) VALUES
(3, 2, 1),
(3, 5, 1),
(3, 7, 0),
(3, 10, 2),
(3, 13, 1),
(3, 20, 2),
(3, 26, 2),
(3, 28, 1),
(3, 29, 2),
(3, 33, 1),
(3, 34, 1),
(3, 43, 1),
(3, 44, 2),
(3, 46, 1),
(3, 49, 1),
(3, 51, 1),
(3, 53, 1),
(3, 56, 2),
(3, 60, 1),
(3, 63, 1),
(3, 69, 1),
(3, 76, 1),
(3, 80, 1),
(3, 82, 1),
(3, 83, 1),
(3, 84, 1),
(3, 91, 2),
(3, 93, 1),
(3, 97, 1),
(3, 102, 1),
(3, 105, 1),
(3, 113, 1),
(3, 116, 1),
(3, 118, 1),
(3, 124, 1),
(3, 132, 1),
(3, 135, 1),
(3, 137, 1),
(3, 142, 1),
(3, 144, 1),
(3, 145, 1),
(3, 148, 1),
(3, 149, 1),
(3, 150, 3),
(3, 154, 1),
(3, 155, 1),
(3, 160, 1),
(3, 162, 2),
(5, 25, 1),
(5, 49, 1),
(5, 69, 1),
(5, 94, 1),
(5, 95, 1),
(5, 96, 1),
(5, 97, 1),
(5, 110, 1),
(5, 139, 1),
(5, 155, 1),
(6, 7, 2),
(6, 20, 0),
(6, 53, 1),
(6, 54, 1),
(6, 55, 1),
(6, 58, 1),
(6, 63, 1),
(6, 72, 1),
(6, 109, 1),
(6, 114, 1),
(6, 136, 1),
(6, 138, 1),
(6, 147, 1),
(6, 150, 1),
(6, 155, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `compra`
--

CREATE TABLE `compra` (
  `IdCompra` int(11) NOT NULL,
  `IdUsuario` int(11) NOT NULL,
  `IdPack` int(11) NOT NULL,
  `Fecha` datetime NOT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `compra`
--

INSERT INTO `compra` (`IdCompra`, `IdUsuario`, `IdPack`, `Fecha`, `Estado`) VALUES
(47, 1, 1, '2025-07-21 20:45:30', 1),
(48, 1, 4, '2025-07-21 21:13:03', 1),
(49, 3, 1, '2025-07-22 10:09:30', 1),
(50, 1, 1, '2025-07-22 17:32:58', 1),
(51, 1, 2, '2025-07-23 15:48:41', 1),
(52, 1, 3, '2025-07-23 15:50:15', 1),
(53, 2, 4, '2025-07-23 15:58:18', 1),
(54, 3, 1, '2025-07-28 17:36:25', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `compra_carta`
--

CREATE TABLE `compra_carta` (
  `IdCompra` int(11) NOT NULL,
  `IdCarta` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `compra_carta`
--

INSERT INTO `compra_carta` (`IdCompra`, `IdCarta`, `Cantidad`) VALUES
(47, 29, 1),
(47, 83, 1),
(47, 97, 1),
(47, 124, 1),
(47, 149, 1),
(48, 5, 1),
(48, 10, 1),
(48, 26, 1),
(48, 53, 1),
(48, 56, 1),
(48, 80, 1),
(48, 91, 1),
(48, 132, 1),
(48, 135, 1),
(48, 142, 1),
(48, 144, 1),
(48, 148, 1),
(48, 150, 1),
(48, 154, 1),
(48, 155, 1),
(49, 69, 1),
(49, 94, 1),
(49, 96, 1),
(49, 110, 1),
(49, 139, 1),
(50, 2, 1),
(50, 20, 1),
(50, 46, 1),
(50, 63, 1),
(50, 113, 1),
(51, 7, 1),
(51, 28, 1),
(51, 43, 1),
(51, 49, 1),
(51, 51, 1),
(51, 82, 1),
(51, 105, 1),
(52, 13, 1),
(52, 26, 1),
(52, 69, 1),
(52, 76, 1),
(52, 93, 1),
(52, 116, 1),
(52, 137, 1),
(52, 145, 1),
(52, 160, 1),
(52, 162, 1),
(53, 7, 1),
(53, 20, 1),
(53, 53, 1),
(53, 54, 1),
(53, 55, 1),
(53, 58, 1),
(53, 63, 1),
(53, 72, 1),
(53, 109, 1),
(53, 114, 1),
(53, 136, 1),
(53, 138, 1),
(53, 147, 1),
(53, 150, 1),
(53, 155, 1),
(54, 25, 1),
(54, 49, 1),
(54, 95, 1),
(54, 97, 1),
(54, 155, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `intercambio`
--

CREATE TABLE `intercambio` (
  `IdIntercambio` int(11) NOT NULL,
  `IdUsuarioEmisor` int(11) NOT NULL,
  `IdUsuarioReceptor` int(11) NOT NULL,
  `IdColeccionEmisor` int(11) NOT NULL,
  `IdColeccionReceptor` int(11) NOT NULL,
  `Fecha` datetime NOT NULL,
  `Estado` tinyint(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `intercambio`
--

INSERT INTO `intercambio` (`IdIntercambio`, `IdUsuarioEmisor`, `IdUsuarioReceptor`, `IdColeccionEmisor`, `IdColeccionReceptor`, `Fecha`, `Estado`) VALUES
(4, 1, 2, 3, 6, '2025-07-26 15:35:00', 1),
(6, 1, 3, 3, 5, '2025-07-28 19:50:14', 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `intercambio_carta`
--

CREATE TABLE `intercambio_carta` (
  `IdIntercambio` int(11) NOT NULL,
  `IdCarta` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL,
  `EsDeEmisor` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `intercambio_carta`
--

INSERT INTO `intercambio_carta` (`IdIntercambio`, `IdCarta`, `Cantidad`, `EsDeEmisor`) VALUES
(4, 7, 1, 1),
(4, 20, 1, 0),
(6, 10, 1, 1),
(6, 25, 1, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pack`
--

CREATE TABLE `pack` (
  `IdPack` int(11) NOT NULL,
  `Nombre` varchar(200) NOT NULL,
  `Precio` int(11) NOT NULL,
  `TotalCartas` int(11) NOT NULL,
  `Imagen` varchar(200) NOT NULL,
  `Leyenda` varchar(200) NOT NULL,
  `RaraChance` decimal(5,2) NOT NULL,
  `EpicaChance` decimal(5,2) NOT NULL,
  `LegendariaChance` decimal(5,2) NOT NULL,
  `RaraGar` int(11) NOT NULL,
  `EpicaGar` int(11) NOT NULL,
  `LegGar` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pack`
--

INSERT INTO `pack` (`IdPack`, `Nombre`, `Precio`, `TotalCartas`, `Imagen`, `Leyenda`, `RaraChance`, `EpicaChance`, `LegendariaChance`, `RaraGar`, `EpicaGar`, `LegGar`) VALUES
(1, 'Basico', 5000, 5, '/PacksArte/pack_1.png', ' 5 cartas aleatorias', 3.00, 2.00, 0.25, 0, 0, 0),
(2, 'Raro', 10000, 7, '/PacksArte/Raro.png', '7 cartas 1 rara garantizada', 4.00, 3.00, 0.35, 1, 0, 0),
(3, 'Epico', 15000, 10, '/PacksArte/Epico.png', '10 cartas 2 raras garantizadas y chance rareza +', 5.00, 4.00, 0.35, 2, 0, 0),
(4, 'Jumbo', 20000, 15, '/PacksArte/pack_4.png', '15 cartas 3 raras garantizadas y chance rareza ++', 6.00, 5.00, 0.55, 3, 0, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo`
--

CREATE TABLE `tipo` (
  `IdTipo` int(11) NOT NULL,
  `Descripcion` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipo`
--

INSERT INTO `tipo` (`IdTipo`, `Descripcion`) VALUES
(0, '-'),
(1, 'Normal'),
(2, 'Psiquico'),
(3, 'Agua'),
(4, 'Electrico'),
(5, 'Planta'),
(6, 'Tierra'),
(7, 'Volador'),
(8, 'Peleador'),
(9, 'Fantasma'),
(10, 'Fuego'),
(11, 'Dragon'),
(12, 'Roca'),
(13, 'Hielo'),
(14, 'Bicho'),
(15, 'Veneno');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `IdUsuario` int(11) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Clave` varchar(256) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `DNI` varchar(20) NOT NULL,
  `Avatar` varchar(200) DEFAULT NULL,
  `Rol` varchar(50) NOT NULL,
  `PuntosVirtuales` int(11) NOT NULL DEFAULT 100,
  `Apellido` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`IdUsuario`, `Email`, `Clave`, `Nombre`, `DNI`, `Avatar`, `Rol`, `PuntosVirtuales`, `Apellido`) VALUES
(1, 'admin@admin.com', 'EwLuHheZASumwcIx1CsEAKaYoy0UxsMaGUxj+Ri/1DQ=', 'Ramon', '12345', '/Uploads/avatar_1.png', 'Administrador', 9492234, 'Alcaraz'),
(2, 'claudio@gmail.com', '+b8lBAGauq80OdALZF/iHcvhsQkBuqqLZ8jsFv6WGgo=', 'Claudio', '37459999', '/Uploads/avatar_2.jpg', 'Usuario', 480000, 'Garro'),
(3, 'Milton@gmail.com', 'EwLuHheZASumwcIx1CsEAKaYoy0UxsMaGUxj+Ri/1DQ=', 'Milton', '124577', '/Uploads/avatar_3.jpg', 'Usuario', 495000, 'Garcia');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `carta`
--
ALTER TABLE `carta`
  ADD PRIMARY KEY (`IdCarta`),
  ADD KEY `IdCategoria` (`IdCategoria`),
  ADD KEY `IdTipo` (`IdTipo1`),
  ADD KEY `IdTipo2` (`IdTipo2`);

--
-- Indices de la tabla `categoria`
--
ALTER TABLE `categoria`
  ADD PRIMARY KEY (`IdCategoria`);

--
-- Indices de la tabla `coleccion`
--
ALTER TABLE `coleccion`
  ADD PRIMARY KEY (`IdColeccion`),
  ADD KEY `IdUsuario` (`IdUsuario`);

--
-- Indices de la tabla `coleccion_carta`
--
ALTER TABLE `coleccion_carta`
  ADD PRIMARY KEY (`IdColeccion`,`IdCarta`),
  ADD KEY `IdCarta` (`IdCarta`);

--
-- Indices de la tabla `compra`
--
ALTER TABLE `compra`
  ADD PRIMARY KEY (`IdCompra`),
  ADD KEY `IdUsuario` (`IdUsuario`),
  ADD KEY `IdPack` (`IdPack`);

--
-- Indices de la tabla `compra_carta`
--
ALTER TABLE `compra_carta`
  ADD PRIMARY KEY (`IdCompra`,`IdCarta`),
  ADD KEY `IdCarta` (`IdCarta`);

--
-- Indices de la tabla `intercambio`
--
ALTER TABLE `intercambio`
  ADD PRIMARY KEY (`IdIntercambio`),
  ADD KEY `IdOferente` (`IdUsuarioEmisor`),
  ADD KEY `IdReceptor` (`IdUsuarioReceptor`),
  ADD KEY `IdColeccionEmisor` (`IdColeccionEmisor`),
  ADD KEY `IdColeccionReceptor` (`IdColeccionReceptor`);

--
-- Indices de la tabla `intercambio_carta`
--
ALTER TABLE `intercambio_carta`
  ADD PRIMARY KEY (`IdIntercambio`,`IdCarta`,`EsDeEmisor`),
  ADD KEY `idx_carta` (`IdCarta`);

--
-- Indices de la tabla `pack`
--
ALTER TABLE `pack`
  ADD PRIMARY KEY (`IdPack`);

--
-- Indices de la tabla `tipo`
--
ALTER TABLE `tipo`
  ADD PRIMARY KEY (`IdTipo`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`IdUsuario`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `carta`
--
ALTER TABLE `carta`
  MODIFY `IdCarta` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=163;

--
-- AUTO_INCREMENT de la tabla `categoria`
--
ALTER TABLE `categoria`
  MODIFY `IdCategoria` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `coleccion`
--
ALTER TABLE `coleccion`
  MODIFY `IdColeccion` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `compra`
--
ALTER TABLE `compra`
  MODIFY `IdCompra` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT de la tabla `intercambio`
--
ALTER TABLE `intercambio`
  MODIFY `IdIntercambio` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `pack`
--
ALTER TABLE `pack`
  MODIFY `IdPack` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `tipo`
--
ALTER TABLE `tipo`
  MODIFY `IdTipo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `IdUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `carta`
--
ALTER TABLE `carta`
  ADD CONSTRAINT `carta_ibfk_1` FOREIGN KEY (`IdCategoria`) REFERENCES `categoria` (`IdCategoria`),
  ADD CONSTRAINT `carta_ibfk_2` FOREIGN KEY (`IdTipo1`) REFERENCES `tipo` (`IdTipo`),
  ADD CONSTRAINT `carta_ibfk_3` FOREIGN KEY (`IdTipo2`) REFERENCES `tipo` (`IdTipo`);

--
-- Filtros para la tabla `coleccion`
--
ALTER TABLE `coleccion`
  ADD CONSTRAINT `coleccion_ibfk_1` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`);

--
-- Filtros para la tabla `coleccion_carta`
--
ALTER TABLE `coleccion_carta`
  ADD CONSTRAINT `coleccion_carta_ibfk_1` FOREIGN KEY (`IdColeccion`) REFERENCES `coleccion` (`IdColeccion`),
  ADD CONSTRAINT `coleccion_carta_ibfk_2` FOREIGN KEY (`IdCarta`) REFERENCES `carta` (`IdCarta`);

--
-- Filtros para la tabla `compra`
--
ALTER TABLE `compra`
  ADD CONSTRAINT `compra_ibfk_1` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `compra_ibfk_2` FOREIGN KEY (`IdPack`) REFERENCES `pack` (`IdPack`);

--
-- Filtros para la tabla `compra_carta`
--
ALTER TABLE `compra_carta`
  ADD CONSTRAINT `compra_carta_ibfk_1` FOREIGN KEY (`IdCompra`) REFERENCES `compra` (`IdCompra`),
  ADD CONSTRAINT `compra_carta_ibfk_2` FOREIGN KEY (`IdCarta`) REFERENCES `carta` (`IdCarta`);

--
-- Filtros para la tabla `intercambio`
--
ALTER TABLE `intercambio`
  ADD CONSTRAINT `intercambio_ibfk_1` FOREIGN KEY (`IdUsuarioEmisor`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `intercambio_ibfk_2` FOREIGN KEY (`IdUsuarioReceptor`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `intercambio_ibfk_3` FOREIGN KEY (`IdColeccionEmisor`) REFERENCES `coleccion` (`IdColeccion`),
  ADD CONSTRAINT `intercambio_ibfk_4` FOREIGN KEY (`IdColeccionReceptor`) REFERENCES `coleccion` (`IdColeccion`);

--
-- Filtros para la tabla `intercambio_carta`
--
ALTER TABLE `intercambio_carta`
  ADD CONSTRAINT `intercambio_carta_ibfk_1` FOREIGN KEY (`IdIntercambio`) REFERENCES `intercambio` (`IdIntercambio`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
