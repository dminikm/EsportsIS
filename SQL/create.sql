-- Drop tables
DROP TABLE IF EXISTS [MatchToPlayer];
DROP TABLE IF EXISTS [TrainingToPlayer];
DROP TABLE IF EXISTS [MatchResult];
DROP TABLE IF EXISTS [TrainingResult];
DROP TABLE IF EXISTS [TeamToPlayer];
DROP TABLE IF EXISTS [Team];
DROP TABLE IF EXISTS [User];

-- Drop functions
DROP FUNCTION IF EXISTS dbo.get_user_role;

-- Create tables
-- User table
-- Can either be Player | Coach | Manager
CREATE TABLE [User] (
    [user_id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [first_name] VARCHAR(30) NOT NULL,
    [last_name] VARCHAR(30) NOT NULL,
    [login] VARCHAR(7) NOT NULL,
    [password] VARCHAR(40) NOT NULL,
    [role] VARCHAR(10) NOT NULL CHECK ([role] in ('player', 'coach', 'manager')),

    UNIQUE([login])
);

-- Create functions
GO

CREATE FUNCTION get_user_role(@user_id INT)
RETURNS VARCHAR(10)
AS BEGIN
    RETURN (SELECT [role] FROM [User] WHERE [User].[user_id] = @user_id);
END;

GO

-- Team table
-- M:N Relations to User (Player)
CREATE TABLE [Team] (
    [team_id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    [coach_id] INT NULL FOREIGN KEY REFERENCES [User]([user_id]),
    [name] VARCHAR(30) NOT NULL,
    [game] VARCHAR(30) NOT NULL,

    CHECK (
        dbo.get_user_role([coach_id]) = 'coach'
    )
);

-- Team (m) : (n) User (player) connecting table
CREATE TABLE [TeamToPlayer] (
    [player_id] INT NOT NULL FOREIGN KEY REFERENCES [User]([user_id]),
    [team_id] INT NOT NULL FOREIGN KEY REFERENCES [Team]([team_id]),

    PRIMARY KEY ([player_id], [team_id]),

    CHECK (
        dbo.get_user_role([player_id]) = 'player'
    )
);

-- Training result
-- M:N Relations to User (Player)
CREATE TABLE [TrainingResult] (
    [training_id] INT PRIMARY KEY IDENTITY(1, 1),
);

-- TrainingResult (m) : (n) User (player) connecting table
CREATE TABLE [TrainingToPlayer] (
    [player_id] INT NOT NULL FOREIGN KEY REFERENCES [User]([user_id]),
    [training_id] INT NOT NULL FOREIGN KEY REFERENCES [TrainingResult]([training_id]),

    [accuracy] FLOAT NOT NULL CHECK ([accuracy] >= 0.0 AND [accuracy] <= 1.0),
    [speed] INT NOT NULL,
    [reaction_time] INT NOT NULL,

    PRIMARY KEY ([player_id], [training_id]),

    CHECK (
        dbo.get_user_role([player_id]) = 'coach'
    )
);

-- Match result
-- M:N Relations to User (Player)
CREATE TABLE [MatchResult] (
    [match_id] INT PRIMARY KEY IDENTITY(1, 1),
);

-- MatchResult (m) : (n) User (player) connecting table
CREATE TABLE [MatchToPlayer] (
    [player_id] INT NOT NULL FOREIGN KEY REFERENCES [User]([user_id]),
    [match_id] INT NOT NULL FOREIGN KEY REFERENCES [MatchResult]([match_id]),

    [kills] FLOAT NOT NULL,
    [deats] INT NOT NULL,
    [assists] INT NOT NULL,

    PRIMARY KEY ([player_id], [match_id]),

    CHECK (
        dbo.get_user_role([player_id]) = 'player'
    )
);