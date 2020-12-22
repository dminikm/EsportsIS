-- All Users
SELECT * FROM [User];

-- Teams and their players
SELECT
    [User].[first_name], [User].[last_name], [User].[login], [Team].[name] as Team
FROM
    [TeamToPlayer]
JOIN
    [Team]
ON
    [Team].[team_id] = [TeamToPlayer].[team_id]
JOIN
    [User]
ON
    [User].[user_id] = [TeamToPlayer].[player_id];

