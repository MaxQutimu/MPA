USE [master]
GO

/****** Object:  Database [Jukebox]    Script Date: 22.05.2024 00:31:45 ******/
CREATE DATABASE [Jukebox]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Jukebox', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Jukebox.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Jukebox_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Jukebox_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Jukebox].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Jukebox] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Jukebox] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Jukebox] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Jukebox] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Jukebox] SET ARITHABORT OFF 
GO

ALTER DATABASE [Jukebox] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Jukebox] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Jukebox] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Jukebox] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Jukebox] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Jukebox] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Jukebox] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Jukebox] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Jukebox] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Jukebox] SET  DISABLE_BROKER 
GO

ALTER DATABASE [Jukebox] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Jukebox] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Jukebox] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Jukebox] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Jukebox] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Jukebox] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Jukebox] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Jukebox] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [Jukebox] SET  MULTI_USER 
GO

ALTER DATABASE [Jukebox] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Jukebox] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Jukebox] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Jukebox] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [Jukebox] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Jukebox] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [Jukebox] SET QUERY_STORE = ON
GO

ALTER DATABASE [Jukebox] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

ALTER DATABASE [Jukebox] SET  READ_WRITE 
GO

