## como configurar



* ``` git clone https://github.com/propenster/BancosGrupoMatheus.git ```
* set up directory on your local machine


* Rode o seguinte comando ``` "Add-Migration initial" no Package Manager Console ```

* ``` Rode "Update-Database" no Package Manager Console ```

* ``` Para realizar transações, você precisa registrar uma nova conta -> altere a sua conta principal "contaPrincipalSelecionada": "9440610888" in appsettings.json ```

* Rode o código

* Se necessário, os arquivos de criação do banco estão abaixo:

*SCRPIT CRIAÇAÕ DO BANCO
USE [master]
GO

/****** Object:  Database [BancoGrupoMatheus]    Script Date: 31/01/2022 08:27:39 ******/
CREATE DATABASE [BancoGrupoMatheus]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BancoGrupoMatheus', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS02\MSSQL\DATA\BancoGrupoMatheus.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BancoGrupoMatheus_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS02\MSSQL\DATA\BancoGrupoMatheus_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BancoGrupoMatheus].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [BancoGrupoMatheus] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET ARITHABORT OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [BancoGrupoMatheus] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [BancoGrupoMatheus] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET  DISABLE_BROKER 
GO

ALTER DATABASE [BancoGrupoMatheus] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [BancoGrupoMatheus] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [BancoGrupoMatheus] SET  MULTI_USER 
GO

ALTER DATABASE [BancoGrupoMatheus] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [BancoGrupoMatheus] SET DB_CHAINING OFF 
GO

ALTER DATABASE [BancoGrupoMatheus] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [BancoGrupoMatheus] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [BancoGrupoMatheus] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [BancoGrupoMatheus] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [BancoGrupoMatheus] SET QUERY_STORE = OFF
GO

ALTER DATABASE [BancoGrupoMatheus] SET  READ_WRITE 
GO


*SCRPIT CRIAÇÃO DA TABELA EFMigrationsHistory
USE [BancoGrupoMatheus]
GO

/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 31/01/2022 08:29:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


*SCRIPT CRIAÇÃO TABELA Contas
USE [BancoGrupoMatheus]
GO

/****** Object:  Table [dbo].[Contas]    Script Date: 31/01/2022 08:30:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrimeiroNome] [varchar](max) NULL,
	[Sobrenome] [varchar](max) NULL,
	[Email] [varchar](max) NULL,
	[NumeroConta] [varchar](50) NOT NULL,
	[DataDeCriacao] [date] NOT NULL,
	[DataAtualizacao] [date] NULL,
	[Saldo] [varchar](max) NULL,
	[CNPJ] [varchar](100) NULL,
	[CPF] [varchar](100) NULL,
	[TipoDeConta] [varchar](100) NOT NULL,
	[NumeroDeTelefone] [varchar](max) NULL,
	[Fatura] [varchar](max) NULL,
	[Pin] [varchar](max) NULL,
	[PinStoredHash] [varchar](max) NULL,
	[PinStoredSalt] [varchar](max) NULL,
 CONSTRAINT [PK_Contas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


*SCRIPT CRIAÇÃO TABELA Response
USE [BancoGrupoMatheus]
GO

/****** Object:  Table [dbo].[Response]    Script Date: 31/01/2022 08:30:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Response](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ResponseCode] [varchar](max) NULL,
	[RequestId] [varchar](max) NULL,
	[ResponseMessage] [varchar](max) NULL,
	[Data] [date] NULL,
 CONSTRAINT [PK_Response] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

*SCRIPT CRIAÇÃO TABELA StatusTransacao
USE [BancoGrupoMatheus]
GO

/****** Object:  Table [dbo].[StatusTransacao]    Script Date: 31/01/2022 08:30:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StatusTransacao](
	[Id] [int] NOT NULL,
	[StatusTransacao] [varchar](max) NULL,
 CONSTRAINT [PK_StatusTransacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

*SCRIPT CRIAÇÃO TABELA TipoTransacao
USE [BancoGrupoMatheus]
GO

/****** Object:  Table [dbo].[TipoTransacao]    Script Date: 31/01/2022 08:31:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TipoTransacao](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[tiposTransacao] [varchar](max) NOT NULL,
 CONSTRAINT [PK_TipoTransacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

*SCRIPT CRIAÇÃO TABELA Transacoes
USE [BancoGrupoMatheus]
GO

/****** Object:  Table [dbo].[Transacoes]    Script Date: 31/01/2022 08:31:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Transacoes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransferenciaUniqueId] [varchar](max) NULL,
	[MesTransacao] [varchar](max) NULL,
	[StatusTransacao] [varchar](max) NULL,
	[IsSuccessful] [varchar](max) NULL,
	[OrigemTransacao] [varchar](max) NULL,
	[DestinoTransacao] [varchar](max) NULL,
	[ObservacaoTransacao] [varchar](max) NULL,
	[TipoDeTransacao] [varchar](max) NULL,
	[DataTransacao] [varchar](max) NULL,
	[ValorTransacao] [varchar](max) NULL,
	[SaldoTransacao] [varchar](max) NULL,
	[Fatura] [varchar](max) NULL,
	[TotalFatura] [varchar](max) NULL,
 CONSTRAINT [PK_Transacoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO










