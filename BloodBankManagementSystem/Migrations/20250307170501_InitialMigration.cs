using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBankManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodBanks",
                columns: table => new
                {
                    BloodBankID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodBanks", x => x.BloodBankID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "BloodDonationCamps",
                columns: table => new
                {
                    BloodDonationCampID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodBankID = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CampEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodDonationCamps", x => x.BloodDonationCampID);
                    table.ForeignKey(
                        name: "FK_BloodDonationCamps_BloodBanks_BloodBankID",
                        column: x => x.BloodBankID,
                        principalTable: "BloodBanks",
                        principalColumn: "BloodBankID");
                });

            migrationBuilder.CreateTable(
                name: "BloodInventories",
                columns: table => new
                {
                    BloodInventoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberofBottles = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BloodBankID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodInventories", x => x.BloodInventoryID);
                    table.ForeignKey(
                        name: "FK_BloodInventories_BloodBanks_BloodBankID",
                        column: x => x.BloodBankID,
                        principalTable: "BloodBanks",
                        principalColumn: "BloodBankID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    HospitalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodBankID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.HospitalID);
                    table.ForeignKey(
                        name: "FK_Hospitals_BloodBanks_BloodBankID",
                        column: x => x.BloodBankID,
                        principalTable: "BloodBanks",
                        principalColumn: "BloodBankID");
                });

            migrationBuilder.CreateTable(
                name: "BloodDonors",
                columns: table => new
                {
                    BloodDonorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HBCount = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Disease = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    LastDonationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalDonations = table.Column<int>(type: "int", nullable: false),
                    HospitalID = table.Column<int>(type: "int", nullable: true),
                    BloodDonationCampID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodDonors", x => x.BloodDonorID);
                    table.ForeignKey(
                        name: "FK_BloodDonors_BloodDonationCamps_BloodDonationCampID",
                        column: x => x.BloodDonationCampID,
                        principalTable: "BloodDonationCamps",
                        principalColumn: "BloodDonationCampID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BloodDonors_Hospitals_HospitalID",
                        column: x => x.HospitalID,
                        principalTable: "Hospitals",
                        principalColumn: "HospitalID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonationCamps_BloodBankID",
                table: "BloodDonationCamps",
                column: "BloodBankID");

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonors_BloodDonationCampID",
                table: "BloodDonors",
                column: "BloodDonationCampID");

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonors_HospitalID",
                table: "BloodDonors",
                column: "HospitalID");

            migrationBuilder.CreateIndex(
                name: "IX_BloodInventories_BloodBankID",
                table: "BloodInventories",
                column: "BloodBankID");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_BloodBankID",
                table: "Hospitals",
                column: "BloodBankID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodDonors");

            migrationBuilder.DropTable(
                name: "BloodInventories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BloodDonationCamps");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "BloodBanks");
        }
    }
}
