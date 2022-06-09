-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 13, 2022 at 06:04 PM
-- Server version: 10.4.19-MariaDB
-- PHP Version: 8.0.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `sportske_utakmice`
--

-- --------------------------------------------------------

--
-- Table structure for table `igraci`
--

CREATE TABLE `igraci` (
  `id_igraca` int(11) NOT NULL,
  `ime` varchar(255) NOT NULL,
  `prezime` varchar(255) NOT NULL,
  `godina_rodjenja` varchar(255) NOT NULL,
  `id_tima` int(11) NOT NULL,
  `pozicija` varchar(255) NOT NULL,
  `broj_dresa` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `igraci`
--

INSERT INTO `igraci` (`id_igraca`, `ime`, `prezime`, `godina_rodjenja`, `id_tima`, `pozicija`, `broj_dresa`) VALUES
(1, 'Stephen', 'Curry', '1988', 1, 'Bek', 30),
(3, 'LeBron', 'James', '1984', 2, 'Krilo', 6),
(4, 'Davis', 'Anthony', '1993', 2, 'Krilo', 3),
(5, 'Nikola', 'Jokic', '1995', 3, 'Centar', 15),
(6, 'Booker', 'Devin', '1996', 4, 'Bek', 1),
(7, 'Lowry', 'Kyle', '1986', 5, 'Bek', 7),
(15, 'Nemanja', 'Bjelica', '1988', 1, 'Krilo', 8),
(19, 'Romelu', 'Lukaku', '13.05.1993. ', 13, 'napadac', 10);

-- --------------------------------------------------------

--
-- Table structure for table `korisnik`
--

CREATE TABLE `korisnik` (
  `korisnicko_ime` varchar(255) NOT NULL,
  `ime` varchar(255) NOT NULL,
  `prezime` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `korisnik`
--

INSERT INTO `korisnik` (`korisnicko_ime`, `ime`, `prezime`) VALUES
('janjic123', 'Nikola', 'Janjic'),
('petar123', 'Petar', 'Petrovic');

-- --------------------------------------------------------

--
-- Table structure for table `liga`
--

CREATE TABLE `liga` (
  `id_lige` int(11) NOT NULL,
  `id_sporta` int(11) NOT NULL,
  `naziv_lige` varchar(255) NOT NULL,
  `kraj` varchar(255) NOT NULL,
  `pocetak` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `liga`
--

INSERT INTO `liga` (`id_lige`, `id_sporta`, `naziv_lige`, `kraj`, `pocetak`) VALUES
(1, 1, 'NBA', '15.04.2022.', '19.10.2021.'),
(2, 2, 'Liga sampiona', '28.05.2022.', '14.09.2021.');

-- --------------------------------------------------------

--
-- Table structure for table `sport`
--

CREATE TABLE `sport` (
  `id_sporta` int(11) NOT NULL,
  `naziv` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `sport`
--

INSERT INTO `sport` (`id_sporta`, `naziv`) VALUES
(1, 'kosarka'),
(2, 'fudbal');

-- --------------------------------------------------------

--
-- Table structure for table `tim`
--

CREATE TABLE `tim` (
  `id_tima` int(11) NOT NULL,
  `naziv_tima` varchar(255) NOT NULL,
  `godina_osnivanja` int(11) NOT NULL,
  `drzava` varchar(255) NOT NULL,
  `trener` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tim`
--

INSERT INTO `tim` (`id_tima`, `naziv_tima`, `godina_osnivanja`, `drzava`, `trener`) VALUES
(1, 'Golden State Warriors', 1946, 'Kalifornija (SAD)', 'Steve Kerr'),
(2, 'Los Angels Lakers', 1947, 'Kalifornija (SAD)', 'Frank Vogel'),
(3, 'Denver Nuggets', 1967, 'Kolorado (SAD)', 'Michael Malone'),
(4, 'Phoenix Suns', 1968, 'Arizona (SAD)', 'Monty Williams'),
(5, 'Miami Heat', 1988, 'Florida (SAD)', 'Erik Spoelstra'),
(12, 'Real Madrid CF', 1902, 'Španija', ' Karlo Ančeloti'),
(13, 'Chelsea', 1905, 'Engleska', 'Thomas Tuchel'),
(14, 'Liverpool F.C.', 1892, 'K', 'Jirgen Klop');

-- --------------------------------------------------------

--
-- Table structure for table `utakmica`
--

CREATE TABLE `utakmica` (
  `id_utakmice` int(11) NOT NULL,
  `id_lige` int(11) NOT NULL,
  `id_tima1` int(11) NOT NULL,
  `id_tima2` int(11) NOT NULL,
  `datum` varchar(255) NOT NULL,
  `rezultat_tima1` int(11) NOT NULL,
  `rezultat_tima2` int(11) NOT NULL,
  `mesto` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `utakmica`
--

INSERT INTO `utakmica` (`id_utakmice`, `id_lige`, `id_tima1`, `id_tima2`, `datum`, `rezultat_tima1`, `rezultat_tima2`, `mesto`) VALUES
(1, 1, 1, 2, '08.04.2022.', 128, 112, 'Chase Center'),
(4, 1, 1, 5, '06.03.2022.', 118, 104, 'American Airlines Arena'),
(5, 1, 3, 4, '25.03.2022.', 130, 140, 'Pepsi Center'),
(6, 1, 1, 3, '06.03.2022.', 117, 124, 'Crypto.com Arena (Los Angeles)'),
(15, 2, 12, 13, '06.04.2022.', 3, 1, 'Stamford Bridge');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `igraci`
--
ALTER TABLE `igraci`
  ADD PRIMARY KEY (`id_igraca`);

--
-- Indexes for table `korisnik`
--
ALTER TABLE `korisnik`
  ADD PRIMARY KEY (`korisnicko_ime`);

--
-- Indexes for table `liga`
--
ALTER TABLE `liga`
  ADD PRIMARY KEY (`id_lige`);

--
-- Indexes for table `sport`
--
ALTER TABLE `sport`
  ADD PRIMARY KEY (`id_sporta`);

--
-- Indexes for table `tim`
--
ALTER TABLE `tim`
  ADD PRIMARY KEY (`id_tima`);

--
-- Indexes for table `utakmica`
--
ALTER TABLE `utakmica`
  ADD PRIMARY KEY (`id_utakmice`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `igraci`
--
ALTER TABLE `igraci`
  MODIFY `id_igraca` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT for table `liga`
--
ALTER TABLE `liga`
  MODIFY `id_lige` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `sport`
--
ALTER TABLE `sport`
  MODIFY `id_sporta` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `tim`
--
ALTER TABLE `tim`
  MODIFY `id_tima` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT for table `utakmica`
--
ALTER TABLE `utakmica`
  MODIFY `id_utakmice` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
