/*
Copyright ©2009 John Allberg

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/
/* $Id$ */

/****** Object:  Table [dbo].[Competition]    Script Date: 2003-07-07 10:06:27 ******/
CREATE TABLE [dbo].[Competition] (
 [CompetitionId] [int] IDENTITY (1, 1) NOT NULL ,
 [Name] [nvarchar] (50) NOT NULL ,
 [OrganizorClub] [int] NOT NULL ,
 [StartDateTime] [datetime] NULL ,
 [ArenaId] [int] NULL 
) ON [PRIMARY]
GO
 
/****** Object:  Table [dbo].[Arena]    Script Date: 2003-07-07 10:06:28 ******/
CREATE TABLE [dbo].[Arena] (
 [ArenaId] [int] IDENTITY (1, 1) NOT NULL ,
 [Name] [nvarchar] (50) NOT NULL 
) ON [PRIMARY]
GO
 
/****** Object:  Table [dbo].[Clubs]    Script Date: 2003-07-07 10:06:28 ******/
CREATE TABLE [dbo].[Clubs] (
 [ClubId] [int] IDENTITY (1, 1) NOT NULL ,
 [Name] [nvarchar] (50) NOT NULL ,
 [Adr1] [nvarchar] (50) NOT NULL ,
 [ZipCode] [nvarchar] (50) NOT NULL ,
 [City] [nvarchar] (50) NOT NULL 
) ON [PRIMARY]
GO
 
/****** Object:  Table [dbo].[Distances]    Script Date: 2003-07-07 10:06:28 ******/
CREATE TABLE [dbo].[Distances] (
 [DistancesId] [int] IDENTITY (1, 1) NOT NULL ,
 [Distance] [int] NOT NULL ,
 [LanesId] [int] NOT NULL 
) ON [PRIMARY]
GO
 
/****** Object:  Table [dbo].[Lanes]    Script Date: 2003-07-07 10:06:29 ******/
CREATE TABLE [dbo].[Lanes] (
 [LanesId] [int] IDENTITY (1, 1) NOT NULL ,
 [NrOfLanes] [int] NOT NULL ,
 [ArenaId] [int] NOT NULL 
) ON [PRIMARY]
GO
 
/****** Object:  Table [dbo].[Races]    Script Date: 2003-07-07 10:06:30 ******/
CREATE TABLE [dbo].[Races] (
 [RacesId] [int] IDENTITY (1, 1) NOT NULL ,
 [RacesName] [nvarchar] (50) NOT NULL ,
 [DistancesId] [int] NOT NULL ,
 [CompetitionId] [int] NOT NULL ,
 [StartTime] [datetime] NULL ,
 [TimeForEach] [int] NULL 
) ON [PRIMARY]
GO
 
/****** Object:  Table [dbo].[RaceShooters]    Script Date: 2003-07-07 10:06:30 ******/
CREATE TABLE [dbo].[RaceShooters] (
 [Id] [int] IDENTITY (1, 1) NOT NULL ,
 [ShootersId] [int] NOT NULL ,
 [RacesId] [int] NOT NULL ,
 [Result] [int] NOT NULL 
) ON [PRIMARY]
GO
 
/****** Object:  Table [dbo].[Shooter]    Script Date: 2003-07-07 10:06:30 ******/
CREATE TABLE [dbo].[Shooter] (
 [ShooterId] [int] NOT NULL ,
 [Personnr] [nvarchar] (50) NOT NULL ,
 [SurName] [nvarchar] (50) NOT NULL ,
 [GivenName] [nvarchar] (50) NOT NULL ,
 [Email] [nvarchar] (50) NOT NULL ,
 [Adr1] [nvarchar] (50) NOT NULL ,
 [ZipCode] [nvarchar] (50) NOT NULL ,
 [City] [nvarchar] (50) NOT NULL ,
 [ClubId] [int] NOT NULL 
) ON [PRIMARY]
GO
 
ALTER TABLE [dbo].[Competition] WITH NOCHECK ADD 
 CONSTRAINT [PK_Competition] PRIMARY KEY  NONCLUSTERED 
 (
  [CompetitionId]
 )  ON [PRIMARY] 
GO
 
ALTER TABLE [dbo].[Arena] WITH NOCHECK ADD 
 CONSTRAINT [PK_Arena] PRIMARY KEY  NONCLUSTERED 
 (
  [ArenaId]
 )  ON [PRIMARY] 
GO
 
ALTER TABLE [dbo].[Clubs] WITH NOCHECK ADD 
 CONSTRAINT [PK_Clubs] PRIMARY KEY  NONCLUSTERED 
 (
  [ClubId]
 )  ON [PRIMARY] 
GO
 
ALTER TABLE [dbo].[Distances] WITH NOCHECK ADD 
 CONSTRAINT [PK_Distances] PRIMARY KEY  NONCLUSTERED 
 (
  [DistancesId]
 )  ON [PRIMARY] 
GO
 
ALTER TABLE [dbo].[Lanes] WITH NOCHECK ADD 
 CONSTRAINT [PK_Lanes] PRIMARY KEY  NONCLUSTERED 
 (
  [LanesId]
 )  ON [PRIMARY] 
GO
 
ALTER TABLE [dbo].[Races] WITH NOCHECK ADD 
 CONSTRAINT [PK_Races] PRIMARY KEY  NONCLUSTERED 
 (
  [RacesId]
 )  ON [PRIMARY] 
GO
 
ALTER TABLE [dbo].[RaceShooters] WITH NOCHECK ADD 
 CONSTRAINT [PK_RaceShooters] PRIMARY KEY  NONCLUSTERED 
 (
  [Id]
 )  ON [PRIMARY] 
GO
 
ALTER TABLE [dbo].[Shooter] WITH NOCHECK ADD 
 CONSTRAINT [PK_Shooter] PRIMARY KEY  NONCLUSTERED 
 (
  [ShooterId]
 )  ON [PRIMARY] 
GO
 
ALTER TABLE [dbo].[Competition] ADD 
 CONSTRAINT [FK_Competition_Arena] FOREIGN KEY 
 (
  [ArenaId]
 ) REFERENCES [dbo].[Arena] (
  [ArenaId]
 ),
 CONSTRAINT [FK_Competition_Clubs] FOREIGN KEY 
 (
  [OrganizorClub]
 ) REFERENCES [dbo].[Clubs] (
  [ClubId]
 )
GO
 
ALTER TABLE [dbo].[Distances] ADD 
 CONSTRAINT [FK_Distances_Lanes] FOREIGN KEY 
 (
  [LanesId]
 ) REFERENCES [dbo].[Lanes] (
  [LanesId]
 )
GO
 
ALTER TABLE [dbo].[Lanes] ADD 
 CONSTRAINT [FK_Lanes_Arena] FOREIGN KEY 
 (
  [ArenaId]
 ) REFERENCES [dbo].[Arena] (
  [ArenaId]
 )
GO
 
ALTER TABLE [dbo].[Races] ADD 
 CONSTRAINT [FK_Races_Competition] FOREIGN KEY 
 (
  [CompetitionId]
 ) REFERENCES [dbo].[Competition] (
  [CompetitionId]
 ),
 CONSTRAINT [FK_Races_Distances] FOREIGN KEY 
 (
  [DistancesId]
 ) REFERENCES [dbo].[Distances] (
  [DistancesId]
 )
GO
 
ALTER TABLE [dbo].[RaceShooters] ADD 
 CONSTRAINT [FK_RaceShooters_Races] FOREIGN KEY 
 (
  [RacesId]
 ) REFERENCES [dbo].[Races] (
  [RacesId]
 ),
 CONSTRAINT [FK_RaceShooters_Shooter] FOREIGN KEY 
 (
  [ShootersId]
 ) REFERENCES [dbo].[Shooter] (
  [ShooterId]
 )
GO
 
ALTER TABLE [dbo].[Shooter] ADD 
 CONSTRAINT [FK_Shooter_Clubs] FOREIGN KEY 
 (
  [ClubId]
 ) REFERENCES [dbo].[Clubs] (
  [ClubId]
 )
GO
 
