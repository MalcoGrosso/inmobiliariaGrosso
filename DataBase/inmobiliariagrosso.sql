-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 28-10-2022 a las 16:15:20
-- Versión del servidor: 10.4.24-MariaDB
-- Versión de PHP: 8.1.6

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliariagrosso`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `id` int(30) NOT NULL,
  `idInquilino` int(30) NOT NULL,
  `idInmueble` int(30) NOT NULL,
  `desde` date NOT NULL,
  `hasta` date NOT NULL,
  `montoM` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`id`, `idInquilino`, `idInmueble`, `desde`, `hasta`, `montoM`) VALUES
(1, 8, 13, '2022-08-30', '2023-08-30', 4000),
(2, 3, 17, '2022-08-23', '2022-09-21', 15000),
(4, 8, 17, '2022-09-16', '2022-09-23', 2000),
(11, 3, 17, '2023-08-25', '2022-09-19', 150002),
(14, 3, 18, '2022-09-22', '2025-10-22', 55552),
(36, 8, 23, '2022-10-05', '2022-10-10', 5000),
(37, 3, 24, '2022-09-30', '2022-10-21', 15000),
(38, 1, 24, '2022-10-22', '2022-10-28', 1555);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `id` int(50) NOT NULL,
  `direccion` varchar(100) NOT NULL,
  `ambientes` int(50) NOT NULL,
  `superficie` int(50) NOT NULL,
  `latitud` varchar(50) NOT NULL,
  `longitud` varchar(50) NOT NULL,
  `IdPropietario` int(50) NOT NULL,
  `uso` varchar(50) NOT NULL,
  `tipo` varchar(50) NOT NULL,
  `Disponible` tinyint(1) NOT NULL DEFAULT 1,
  `imagen` varchar(100) NOT NULL,
  `precio` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`id`, `direccion`, `ambientes`, `superficie`, `latitud`, `longitud`, `IdPropietario`, `uso`, `tipo`, `Disponible`, `imagen`, `precio`) VALUES
(13, 'rea', 3, 456, '33', '44', 1, 'Casa', 'Residencial', 1, '', 0),
(17, 'dfgdfgdf', 5, 4545, '34', '76', 5, 'Residencial', 'Vivienda', 1, '', 0),
(18, 'Colibri', 2, 23, '1', '33', 8, 'Casa', 'Comercial', 1, '', 0),
(22, 'San Jose 510', 3, 233, '-33.333', '-66.666', 8, 'Departamento', 'Residencial', 0, '', 0),
(23, 'San Martin 126', 2, 250, '33.333', '33.333', 13, 'Casa', 'Residencial', 1, 'https://www.deplace.es/wp-content/uploads/2019/01/que-es-bien-inmueble.jpg', 5000),
(24, 'Lomas', 2, 500, '43', '446', 13, 'Residencial', 'Vivienda', 1, 'https://definicionabc.com/wp-content/uploads/Inmueble-300x225.jpg', 30000),
(80, 'San Juan 234', 3, 500, '-33.2151192', '-66.2351104', 13, 'Casa', 'Residencial', 1, 'http://192.168.1.102:5000/Uploads/inmueble_1380024.PNG', 60000),
(81, 'fjh', 56, 858, '-33.215208', '-66.2348165', 13, 'Casa', 'Residencial', 1, 'http://192.168.1.102:5000/Uploads/inmueble_1328976.PNG', 868),
(82, 'ghh', 6888, 888, '-33.2149939', '-66.2353977', 13, 'Casa', 'Residencial', 1, 'http://192.168.1.102:5000/Uploads/inmueble_1320973.PNG', 888),
(83, 'ghh', 6888, 888, '-33.2149939', '-66.2353977', 13, 'Casa', 'Residencial', 1, 'http://192.168.1.102:5000/Uploads/inmueble_1314270.PNG', 888);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id` int(25) NOT NULL,
  `dni` varchar(50) DEFAULT NULL,
  `nombre` varchar(50) DEFAULT NULL,
  `apellido` varchar(50) DEFAULT NULL,
  `telefono` varchar(50) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`id`, `dni`, `nombre`, `apellido`, `telefono`, `email`) VALUES
(1, '321654654', 'asd', 'asd', '1235', 'asd@sdasd.com'),
(3, '22222222', 'sfsdf', 'fdgdfg', 'dfgdfg', 'fsdfsd@asdasd.com'),
(7, '5434534534', 'aaaaa', 'ppppp', '45345345', 'asdasd@asd111asd.com'),
(8, '54545', 'sdfsdf', 'dfgdfg', '444444444444', 'sdfsd@sadasd.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id` int(11) NOT NULL,
  `IdContrato` int(11) NOT NULL,
  `Fecha` datetime NOT NULL DEFAULT current_timestamp(),
  `Monto` int(11) NOT NULL,
  `NumeroPago` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`Id`, `IdContrato`, `Fecha`, `Monto`, `NumeroPago`) VALUES
(15, 1, '2022-09-16 11:44:57', 3434, 1),
(16, 2, '2022-09-16 13:49:25', 23232, 2),
(21, 14, '2022-09-21 17:59:34', 55552, 1),
(22, 36, '2022-09-29 15:27:37', 5000, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id` int(20) NOT NULL,
  `nombre` varchar(50) DEFAULT NULL,
  `apellido` varchar(50) DEFAULT NULL,
  `dni` varchar(50) DEFAULT NULL,
  `telefono` varchar(50) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `clave` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id`, `nombre`, `apellido`, `dni`, `telefono`, `email`, `clave`) VALUES
(1, 'asd', 'fgsdf', '123123123', '5646456', 'asda@asdasd.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA='),
(5, 'juan', 'perez', '544564', '45645645', 'aaaa@aaa.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA='),
(8, 'Raul', 'Gomez', '435435', '3453453', 'nvbnvb2@asdas.com', ''),
(9, 'dddd', 'dddd', '44444', '33333', 'sdasdasdas@asdasd.com', ''),
(13, 'Mariano', 'Luzza', '123', '123123123', 'fdd@asdas.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA='),
(14, 'ccc', 'CCC', '123123123', '26644444', 'ccc@ccc.com', '123');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) COLLATE utf8mb4_spanish_ci NOT NULL,
  `Apellido` varchar(100) COLLATE utf8mb4_spanish_ci NOT NULL,
  `AvatarUrl` varchar(100) COLLATE utf8mb4_spanish_ci DEFAULT NULL,
  `Email` varchar(100) COLLATE utf8mb4_spanish_ci NOT NULL,
  `Clave` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
  `Rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_spanish_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`Id`, `Nombre`, `Apellido`, `AvatarUrl`, `Email`, `Clave`, `Rol`) VALUES
(8, 'Juan', 'Pablo', '/Uploads\\usuario.png', 'jp@jp.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA=', 2),
(15, 'Malco', 'Grosso', '/Uploads\\avatar_15.jpg', 'm@m.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA=', 2),
(17, 'Raul', 'Gomez', '/Uploads\\usuario.png', 'rg@rg.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA=', 3),
(18, 'Sam', 'Rosa', '/Uploads\\avatar_18.jpg', 'sr@sr.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA=', 2),
(19, 'Raul', 'Gomez', '/Uploads\\avatar_19.jpg', 'pppppp@ppppp.com', 'GAKKw6Co5EiIGNiZC1OfQC6offL+e8CoEs3SX0LIrHA=', 2);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `idInquilino` (`idInquilino`),
  ADD KEY `idInmueble` (`idInmueble`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `IdPropietario` (`IdPropietario`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_IdContrato` (`IdContrato`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `usuario_email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `id` int(30) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `id` int(50) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=84;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id` int(25) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`idInquilino`) REFERENCES `inquilinos` (`id`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`idInmueble`) REFERENCES `inmuebles` (`id`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`IdPropietario`) REFERENCES `propietarios` (`id`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `contratos` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
