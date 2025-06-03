-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 02-06-2025 a las 02:42:09
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

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `coleccion_carta`
--

CREATE TABLE `coleccion_carta` (
  `IdColeccion` int(11) NOT NULL,
  `IdCarta` int(11) NOT NULL,
  `Cantidad` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `compra`
--

CREATE TABLE `compra` (
  `IdCompra` int(11) NOT NULL,
  `IdUsuario` int(11) NOT NULL,
  `IdCarta` int(11) DEFAULT NULL,
  `Fecha` datetime NOT NULL,
  `Precio` int(11) NOT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1,
  `IdCreadoPor` int(11) NOT NULL,
  `IdAnuladoPor` int(11) DEFAULT NULL,
  `Cantidad` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `intercambio`
--

CREATE TABLE `intercambio` (
  `IdIntercambio` int(11) NOT NULL,
  `IdOferente` int(11) NOT NULL,
  `IdReceptor` int(11) NOT NULL,
  `Fecha` datetime NOT NULL,
  `Estado` tinyint(4) NOT NULL,
  `IdCreadoPor` int(11) NOT NULL,
  `IdAnuladoPor` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `intercambio_cartas`
--

CREATE TABLE `intercambio_cartas` (
  `IdIntercambioCarta` int(11) NOT NULL,
  `IdIntercambio` int(11) NOT NULL,
  `IdCarta` int(11) NOT NULL,
  `EsOferente` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tienda`
--

CREATE TABLE `tienda` (
  `IdTienda` int(11) NOT NULL,
  `IdCarta` int(11) NOT NULL,
  `Cantidad` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
(1, 'Normal'),
(2, 'Psiquico'),
(3, 'Agua'),
(4, 'Electrico'),
(5, 'Planca'),
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
(1, 'admin@admin.com', 'EwLuHheZASumwcIx1CsEAKaYoy0UxsMaGUxj+Ri/1DQ=', 'Ramon', '33412102', NULL, 'Administrador', 1000000, 'Alcaraz'),
(2, 'claudio@gmail.com', '+b8lBAGauq80OdALZF/iHcvhsQkBuqqLZ8jsFv6WGgo=', 'Claudio', '37459999', NULL, 'Usuario', 100, 'Garro'),
(3, 'Milton@gmail.com', '+b8lBAGauq80OdALZF/iHcvhsQkBuqqLZ8jsFv6WGgo=', 'Milton', '', '/Uploads/avatar_3.jpg', 'Usuario', 0, 'Garcia');

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
  ADD KEY `IdCarta` (`IdCarta`),
  ADD KEY `IdCreadoPor` (`IdCreadoPor`),
  ADD KEY `IdAnuladoPor` (`IdAnuladoPor`);

--
-- Indices de la tabla `intercambio`
--
ALTER TABLE `intercambio`
  ADD PRIMARY KEY (`IdIntercambio`),
  ADD KEY `IdOferente` (`IdOferente`),
  ADD KEY `IdReceptor` (`IdReceptor`),
  ADD KEY `IdCreadoPor` (`IdCreadoPor`),
  ADD KEY `IdAnuladoPor` (`IdAnuladoPor`);

--
-- Indices de la tabla `intercambio_cartas`
--
ALTER TABLE `intercambio_cartas`
  ADD PRIMARY KEY (`IdIntercambioCarta`),
  ADD UNIQUE KEY `IdIntercambio` (`IdIntercambio`),
  ADD KEY `IdCarta` (`IdCarta`);

--
-- Indices de la tabla `tienda`
--
ALTER TABLE `tienda`
  ADD PRIMARY KEY (`IdTienda`),
  ADD KEY `IdCarta` (`IdCarta`);

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
  MODIFY `IdCarta` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `categoria`
--
ALTER TABLE `categoria`
  MODIFY `IdCategoria` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `coleccion`
--
ALTER TABLE `coleccion`
  MODIFY `IdColeccion` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `compra`
--
ALTER TABLE `compra`
  MODIFY `IdCompra` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `intercambio`
--
ALTER TABLE `intercambio`
  MODIFY `IdIntercambio` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `intercambio_cartas`
--
ALTER TABLE `intercambio_cartas`
  MODIFY `IdIntercambioCarta` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `tienda`
--
ALTER TABLE `tienda`
  MODIFY `IdTienda` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `tipo`
--
ALTER TABLE `tipo`
  MODIFY `IdTipo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

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
  ADD CONSTRAINT `compra_ibfk_2` FOREIGN KEY (`IdCarta`) REFERENCES `carta` (`IdCarta`),
  ADD CONSTRAINT `compra_ibfk_3` FOREIGN KEY (`IdCreadoPor`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `compra_ibfk_4` FOREIGN KEY (`IdAnuladoPor`) REFERENCES `usuario` (`IdUsuario`);

--
-- Filtros para la tabla `intercambio`
--
ALTER TABLE `intercambio`
  ADD CONSTRAINT `intercambio_ibfk_1` FOREIGN KEY (`IdOferente`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `intercambio_ibfk_2` FOREIGN KEY (`IdReceptor`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `intercambio_ibfk_3` FOREIGN KEY (`IdCreadoPor`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `intercambio_ibfk_4` FOREIGN KEY (`IdAnuladoPor`) REFERENCES `usuario` (`IdUsuario`);

--
-- Filtros para la tabla `intercambio_cartas`
--
ALTER TABLE `intercambio_cartas`
  ADD CONSTRAINT `intercambio_cartas_ibfk_1` FOREIGN KEY (`IdIntercambio`) REFERENCES `intercambio` (`IdIntercambio`),
  ADD CONSTRAINT `intercambio_cartas_ibfk_2` FOREIGN KEY (`IdCarta`) REFERENCES `carta` (`IdCarta`);

--
-- Filtros para la tabla `tienda`
--
ALTER TABLE `tienda`
  ADD CONSTRAINT `tienda_ibfk_1` FOREIGN KEY (`IdCarta`) REFERENCES `carta` (`IdCarta`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
