IF DB_ID('TempQuestionsDatabase') IS NULL
	BEGIN
		CREATE DATABASE [TempQuestionsDatabase]
	END
GO

USE [TempQuestionsDatabase]
GO

IF OBJECT_ID(N'[dbo].[AllQuestions]', N'U') IS NULL  
		CREATE TABLE [dbo].[AllQuestions](
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[Type] [nvarchar](50) NOT NULL,
			[Order] [int] NOT NULL,
			[Text] [nvarchar](255) NOT NULL,
		 CONSTRAINT [PK_AllQuestions] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)
		) ON [PRIMARY]
GO

IF OBJECT_ID(N'[dbo].[SliderQuestions]', N'U') IS NULL 
	BEGIN
			CREATE TABLE [dbo].[SliderQuestions](
				[Id] [int] NOT NULL,
				[StartValue] [int] NOT NULL,
				[EndValue] [int] NOT NULL,
				[StartValueCaption] [nvarchar](250) NOT NULL,
				[EndValueCaption] [nvarchar](250) NOT NULL,
			 CONSTRAINT [IX_SliderQuestions] UNIQUE NONCLUSTERED 
			(
				[Id] ASC
			)
			) ON [PRIMARY];

			ALTER TABLE [dbo].[SliderQuestions]  WITH CHECK ADD  CONSTRAINT [FK_SliderQuestions_AllQuestions] FOREIGN KEY([Id])
			REFERENCES [dbo].[AllQuestions] ([Id])

			ALTER TABLE [dbo].[SliderQuestions] CHECK CONSTRAINT [FK_SliderQuestions_AllQuestions]
	END
GO

IF OBJECT_ID(N'[dbo].[SmileyQuestions]', N'U') IS NULL  
	BEGIN
		CREATE TABLE [dbo].[SmileyQuestions](
			[Id] [int] NOT NULL,
			[NumberOfSmiley] [int] NOT NULL,
		 CONSTRAINT [IX_SmileyQuestions] UNIQUE NONCLUSTERED 
		(
			[Id] ASC
		)
		) ON [PRIMARY]

		ALTER TABLE [dbo].[SmileyQuestions]  WITH CHECK ADD  CONSTRAINT [FK_SmileyQuestions_AllQuestions] FOREIGN KEY([Id])
		REFERENCES [dbo].[AllQuestions] ([Id])

		ALTER TABLE [dbo].[SmileyQuestions] CHECK CONSTRAINT [FK_SmileyQuestions_AllQuestions]
	END
GO

IF OBJECT_ID(N'[dbo].[StarQuestions]', N'U') IS NULL  
	BEGIN
		CREATE TABLE [dbo].[StarQuestions](
			[Id] [int] NOT NULL,
			[NumberOfStar] [int] NOT NULL,
		 CONSTRAINT [IX_StarQuestions] UNIQUE NONCLUSTERED 
		(
			[Id] ASC
		)
		) ON [PRIMARY]

		ALTER TABLE [dbo].[StarQuestions]  WITH CHECK ADD  CONSTRAINT [FK_StarQuestions_AllQuestions] FOREIGN KEY([Id])
		REFERENCES [dbo].[AllQuestions] ([Id])

		ALTER TABLE [dbo].[StarQuestions] CHECK CONSTRAINT [FK_StarQuestions_AllQuestions]
	END
GO

CREATE OR ALTER PROCEDURE [dbo].Add_StarQuestions
 (@Text nvarchar(250), @Order INT, @NumberOfStar INT, @Id INT = NULL OUTPUT)
 AS  
 BEGIN   
	SET XACT_ABORT ON;
	INSERT INTO AllQuestions VALUES('Star', @Order, @Text);
	SET @Id = SCOPE_IDENTITY();
	INSERT INTO StarQuestions VALUES(@Id, @NumberOfStar);
END  
GO

CREATE OR ALTER PROCEDURE [dbo].Add_SliderQuestions  
 (    @Text nvarchar(250),    @Order  INT,    @StartValue INT,    @EndValue INT,    @StartValueCaption nvarchar(250),    @EndValueCaption nvarchar(250), @Id INT = NULL OUTPUT)  
 AS  
 BEGIN   
	SET XACT_ABORT ON;
	INSERT INTO AllQuestions VALUES('Slider', @Order, @Text);
	SET @Id = SCOPE_IDENTITY();
	INSERT INTO SliderQuestions VALUES(@Id, @StartValue, @EndValue, @StartValueCaption, @EndValueCaption);
END 
GO

CREATE OR ALTER PROCEDURE [dbo].Add_SmileyQuestions  
 (    @Text nvarchar(255),    @Order  INT,    @NumberOfSmiley INT, @Id INT = NULL OUTPUT)  
 AS  
 BEGIN   
	SET XACT_ABORT ON;
	INSERT INTO AllQuestions VALUES('Smiley', @Order, @Text);
	SET @Id = SCOPE_IDENTITY();
	INSERT INTO SmileyQuestions VALUES(@Id, @NumberOfSmiley);
END  
GO

CREATE OR ALTER PROCEDURE [dbo].Update_StarQuestions  
(    @Text nvarchar(255),    @Order  INT,    @NumberOfStar INT,    @Id INT  )  
AS  
BEGIN   
	 SET XACT_ABORT ON;
	 UPDATE AllQuestions SET Text = @Text, [Order] = @Order WHERE Id = @Id AND Type = 'Star'; 
	 UPDATE StarQuestions SET NumberOfStar = @NumberOfStar WHERE Id = @Id;   
END 
GO

CREATE OR ALTER PROCEDURE [dbo].Update_SliderQuestions  
(    @Text nvarchar(250),    @Order  INT,    @StartValue INT,    @EndValue INT,    @StartValueCaption nvarchar(250),    @EndValueCaption nvarchar(250),    @Id INT  )
AS  
BEGIN   
	SET XACT_ABORT ON;      
	UPDATE AllQuestions SET Text = @Text, [Order] = @Order WHERE Id = @Id AND Type = 'Slider'; 
	UPDATE SliderQuestions SET StartValue = @StartValue, EndValue = @EndValue, StartValueCaption = @StartValueCaption, EndValueCaption = @EndValueCaption WHERE Id = @Id;   
END
GO

CREATE OR ALTER PROCEDURE [dbo].Update_SmileyQuestions  
(    @Text nvarchar(255),    @Order  INT,    @NumberOfSmiley INT,    @Id INT  )  
AS  
BEGIN   
	SET XACT_ABORT ON;      
	UPDATE AllQuestions SET Text = @Text, [Order] = @Order WHERE Id = @Id AND Type = 'Smiley';  
	UPDATE SmileyQuestions SET NumberOfSmiley = @NumberOfSmiley WHERE Id = @Id;   
END 
GO

CREATE OR ALTER PROCEDURE [dbo].Delete_StarQuestions  
(    @Id INT  )
AS  
BEGIN   
	SET XACT_ABORT ON;      
	DELETE FROM StarQuestions WHERE Id = @Id;   
	DELETE FROM AllQuestions WHERE Id = @Id AND Type = 'Star';  
END  
GO

CREATE OR ALTER PROCEDURE [dbo].Delete_SliderQuestions
(    @Id INT  )  
AS  
BEGIN   
	SET XACT_ABORT ON;      
	DELETE FROM SliderQuestions WHERE Id = @Id;   
	DELETE FROM AllQuestions WHERE Id = @Id AND Type = 'Slider';  
END
GO

CREATE OR ALTER PROCEDURE [dbo].Delete_SmileyQuestions  
(    @Id INT  )
AS  
BEGIN   
	SET XACT_ABORT ON;      
	DELETE FROM SmileyQuestions WHERE Id = @Id;   
	DELETE FROM AllQuestions WHERE Id = @Id AND Type = 'Smiley';  
END  
GO

CREATE OR ALTER PROCEDURE [dbo].Get_SliderQuestions
	(@Id INT)
	AS
	BEGIN
		SELECT AllQuestions.Type, AllQuestions.[Order], AllQuestions.Text, SliderQuestions.StartValue, SliderQuestions.EndValue, SliderQuestions.StartValueCaption, SliderQuestions.EndValueCaption
		FROM AllQuestions
		INNER JOIN SliderQuestions ON AllQuestions.Id = SliderQuestions.Id
		WHERE AllQuestions.Id = @Id;
	END
GO

CREATE OR ALTER PROCEDURE [dbo].Get_SmileyQuestions
	(@Id INT)
	AS
	BEGIN
		SELECT AllQuestions.Type, AllQuestions.[Order], AllQuestions.Text, SmileyQuestions.NumberOfSmiley
		FROM AllQuestions
		INNER JOIN SmileyQuestions ON AllQuestions.Id = SmileyQuestions.Id
		WHERE AllQuestions.Id = @Id;
	END
GO

CREATE OR ALTER PROCEDURE [dbo].Get_StarQuestions
	(@Id INT)
	AS
	BEGIN
		SELECT AllQuestions.Type, AllQuestions.[Order], AllQuestions.Text, StarQuestions.NumberOfStar
		FROM AllQuestions
		INNER JOIN StarQuestions ON AllQuestions.Id = StarQuestions.Id
		WHERE AllQuestions.Id = @Id;
	END
GO