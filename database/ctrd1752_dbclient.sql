-- phpMyAdmin SQL Dump
-- version 4.9.5
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Waktu pembuatan: 07 Jun 2021 pada 15.58
-- Versi server: 10.3.29-MariaDB-cll-lve
-- Versi PHP: 7.3.28

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ctrd1752_dbclient`
--

-- --------------------------------------------------------

--
-- Struktur dari tabel `tclient`
--

CREATE TABLE `tclient` (
  `id` varchar(50) NOT NULL,
  `code` varchar(150) NOT NULL,
  `name` varchar(255) NOT NULL,
  `contact_person` varchar(255) NOT NULL,
  `hp` varchar(15) NOT NULL,
  `address` varchar(255) NOT NULL,
  `license` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Struktur dari tabel `tclientd`
--

CREATE TABLE `tclientd` (
  `id` varchar(50) NOT NULL,
  `idclient` varchar(50) NOT NULL,
  `pc_id` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indexes for dumped tables
--

--
-- Indeks untuk tabel `tclient`
--
ALTER TABLE `tclient`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ix_tclient_code` (`code`);

--
-- Indeks untuk tabel `tclientd`
--
ALTER TABLE `tclientd`
  ADD PRIMARY KEY (`id`),
  ADD KEY `ix_tclientd_pc_id` (`pc_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
