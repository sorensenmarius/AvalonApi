using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiplayerAvalon.Migrations
{
    public partial class AddStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var procs = @"
							GO
								CREATE OR ALTER PROCEDURE [dbo].[VoteForTeam]
									(
										@RoundId uniqueidentifier = NULL,
										@TotalVotes int output
									)
									AS
										UPDATE Rounds SET VotesForTeam = VotesForTeam + 1 WHERE Id = @RoundId
										SET @TotalVotes = (SELECT VotesForTeam + VotesAgainstTeam AS TotalVotes FROM Rounds WHERE Id = @RoundId)

							GO
								CREATE OR ALTER PROCEDURE [dbo].[VoteAgainstTeam]
									(
										@RoundId uniqueidentifier = NULL,
										@TotalVotes int output
									)
									AS
										UPDATE Rounds SET VotesAgainstTeam = VotesAgainstTeam + 1 WHERE Id = @RoundId
										SET @TotalVotes = (SELECT VotesForTeam + VotesAgainstTeam AS TotalVotes FROM Rounds WHERE Id = @RoundId)
	
							GO
								CREATE OR ALTER PROCEDURE [dbo].[VoteForExpedition]
									(
										@RoundId uniqueidentifier = NULL,
										@TotalVotes int output
									)
									AS
										UPDATE Rounds SET MissionVoteGood = MissionVoteGood + 1 WHERE Id = @RoundId
										SET @TotalVotes = (SELECT MissionVoteBad + MissionVoteGood AS TotalVotes FROM Rounds WHERE Id = @RoundId)

							GO
								CREATE OR ALTER PROCEDURE [dbo].[VoteAgainstExpedition]
									(
										@RoundId uniqueidentifier = NULL,
										@TotalVotes int output
									)
									AS
										UPDATE Rounds SET MissionVoteBad = MissionVoteBad + 1 WHERE Id = @RoundId
										SET @TotalVotes = (SELECT MissionVoteBad + MissionVoteGood AS TotalVotes FROM Rounds WHERE Id = @RoundId)
			


			
                        ";
            migrationBuilder.Sql(procs);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
