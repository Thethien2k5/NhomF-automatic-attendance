-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th10 16, 2024 lúc 01:14 AM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `automaticattendancesoftware`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `attendance`
--

CREATE TABLE `attendance` (
  `IDStudent` int(9) NOT NULL,
  `NameStudent` varchar(50) NOT NULL,
  `DateAttendance` date NOT NULL,
  `Status` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `manager`
--

CREATE TABLE `manager` (
  `IDManager` int(9) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `ManagementLevel` tinyint(1) NOT NULL,
  `Class` varchar(7) NOT NULL,
  `ManagerName` varchar(50) NOT NULL,
  `Pass` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `manager`
--

INSERT INTO `manager` (`IDManager`, `Name`, `ManagementLevel`, `Class`, `ManagerName`, `Pass`) VALUES
(124585832, 'Giáo Viên 3', 0, 'CNPM2k4', 'Giaovien1@ut.edu.vn', 'sdfghjkl'),
(325783449, 'Ban Giám Hiệu 1', 1, '', 'Bangiamhieu1@ut.edu.vn', '12345678'),
(346736575, 'Giáo Viên 4', 0, 'CNPM2k6', 'Giaovien1@ut.edu.vn', 'zxcvbnma'),
(455256557, 'Ban Giám Hiệu 2', 1, '', 'Bangiamhieu2@ut.edu.vn', '89012345'),
(536459674, 'Giáo Viên 1', 0, 'CNPM2k3', 'Giaovien1@ut.edu.vn', 'qwertyui'),
(579733587, 'Giáo Viên 2', 0, 'CNPM2k5', 'Giaovien1@ut.edu.vn', 'poiuytre');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `studentinformation`
--

CREATE TABLE `studentinformation` (
  `IDStudent` int(9) NOT NULL,
  `NameStudent` text NOT NULL,
  `Class` text NOT NULL,
  `Parents` text NOT NULL,
  `ParentsEmail` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Đang đổ dữ liệu cho bảng `studentinformation`
--

INSERT INTO `studentinformation` (`IDStudent`, `NameStudent`, `Class`, `Parents`, `ParentsEmail`) VALUES
(10054, 'Student54', 'CNPM2k5', 'Parent54', 'thethien2k5@gmail.com'),
(10055, 'Student55', 'CNPM2k5', 'Parent55', 'thethien2k5@gmail.com'),
(10056, 'Student56', 'CNPM2k5', 'Parent56', 'thethien2k5@gmail.com'),
(10057, 'Student57', 'CNPM2k5', 'Parent57', 'thethien2k5@gmail.com'),
(10058, 'Student58', 'CNPM2k5', 'Parent58', 'thethien2k5@gmail.com'),
(10059, 'Student59', 'CNPM2k5', 'Parent59', 'thethien2k5@gmail.com'),
(10060, 'Student60', 'CNPM2k5', 'Parent60', 'thethien2k5@gmail.com'),
(10107, 'Student107', 'CNPM2k6', 'Parent107', 'thethien2k5@gmail.com'),
(10108, 'Student108', 'CNPM2k6', 'Parent108', 'thethien2k5@gmail.com'),
(10109, 'Student109', 'CNPM2k6', 'Parent109', 'thethien2k5@gmail.com'),
(10110, 'Student110', 'CNPM2k6', 'Parent110', 'thethien2k5@gmail.com'),
(10111, 'Student111', 'CNPM2k6', 'Parent111', 'thethien2k5@gmail.com'),
(10112, 'Student112', 'CNPM2k6', 'Parent112', 'thethien2k5@gmail.com'),
(10113, 'Student113', 'CNPM2k6', 'Parent113', 'thethien2k5@gmail.com'),
(10170, 'Student170', 'CNPM2k4', 'Parent170', 'thethien2k5@gmail.com'),
(10171, 'Student171', 'CNPM2k4', 'Parent171', 'thethien2k5@gmail.com'),
(10172, 'Student172', 'CNPM2k4', 'Parent172', 'thethien2k5@gmail.com'),
(10173, 'Student173', 'CNPM2k4', 'Parent173', 'thethien2k5@gmail.com'),
(10174, 'Student174', 'CNPM2k4', 'Parent174', 'thethien2k5@gmail.com'),
(10175, 'Student175', 'CNPM2k4', 'Parent175', 'thethien2k5@gmail.com'),
(10176, 'Student176', 'CNPM2k4', 'Parent176', 'thethien2k5@gmail.com'),
(10234, 'Student234', 'CNPM2k3', 'Parent234', 'thethien2k5@gmail.com'),
(10235, 'Student235', 'CNPM2k3', 'Parent235', 'thethien2k5@gmail.com'),
(10236, 'Student236', 'CNPM2k3', 'Parent236', 'thethien2k5@gmail.com'),
(10237, 'Student237', 'CNPM2k3', 'Parent237', 'thethien2k5@gmail.com'),
(10238, 'Student238', 'CNPM2k3', 'Parent238', 'thethien2k5@gmail.com'),
(10239, 'Student239', 'CNPM2k3', 'Parent239', 'thethien2k5@gmail.com'),
(10240, 'Student240', 'CNPM2k3', 'Parent240', 'thethien2k5@gmail.com');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `attendance`
--
ALTER TABLE `attendance`
  ADD PRIMARY KEY (`IDStudent`),
  ADD UNIQUE KEY `NameStudent` (`NameStudent`);

--
-- Chỉ mục cho bảng `manager`
--
ALTER TABLE `manager`
  ADD PRIMARY KEY (`IDManager`);

--
-- Chỉ mục cho bảng `studentinformation`
--
ALTER TABLE `studentinformation`
  ADD PRIMARY KEY (`IDStudent`),
  ADD UNIQUE KEY `NameStudent` (`NameStudent`) USING HASH;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `attendance`
--
ALTER TABLE `attendance`
  ADD CONSTRAINT `attendance_ibfk_1` FOREIGN KEY (`IDStudent`) REFERENCES `studentinformation` (`IDStudent`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
