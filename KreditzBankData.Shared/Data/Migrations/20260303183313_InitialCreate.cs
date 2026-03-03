using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KreditzBankData.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BankId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ingestion_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FileHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecordsProcessed = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingestion_logs", x => x.Id);
                    table.UniqueConstraint("AK_ingestion_logs_BatchId", x => x.BatchId);
                });

            migrationBuilder.CreateTable(
                name: "account_holders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_holders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_account_holders_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "balances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_balances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_balances_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_balances_ingestion_logs_BatchId",
                        column: x => x.BatchId,
                        principalTable: "ingestion_logs",
                        principalColumn: "BatchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TransactionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsAnomaly = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AnomalyReasons = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactions_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_ingestion_logs_BatchId",
                        column: x => x.BatchId,
                        principalTable: "ingestion_logs",
                        principalColumn: "BatchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_holders_account_id",
                table: "account_holders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "ix_account_account_number",
                table: "accounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_balances_account_id_created_at",
                table: "balances",
                columns: new[] { "AccountId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "ix_balances_batch_id",
                table: "balances",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "ix_categories_is_active",
                table: "categories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "ix_ingestion_logs_batch_id",
                table: "ingestion_logs",
                column: "BatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ingestion_logs_file_hash",
                table: "ingestion_logs",
                column: "FileHash");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_account_id_external_id",
                table: "transactions",
                columns: new[] { "AccountId", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transactions_account_id_transaction_date",
                table: "transactions",
                columns: new[] { "AccountId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "ix_transactions_batch_id",
                table: "transactions",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_category_id",
                table: "transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_is_anomaly",
                table: "transactions",
                column: "IsAnomaly");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_holders");

            migrationBuilder.DropTable(
                name: "balances");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "ingestion_logs");
        }
    }
}
