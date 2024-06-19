[![.NET](https://github.com/ArtemStepanov/Stushbr/actions/workflows/ci-dotnet.yml/badge.svg)](https://github.com/ArtemStepanov/Stushbr/actions/workflows/ci-dotnet.yml)
[![NPM Build PaymentsGatewayWeb](https://github.com/ArtemStepanov/Stushbr/actions/workflows/ci-node.yml/badge.svg)](https://github.com/ArtemStepanov/Stushbr/actions/workflows/ci-node.yml)

# Stushbr

## Description

Stushbr is a business process management project showcasing my approach to development. Key features include a Payment Gateway (QIWI) and Entities Processor.

## Requirements

- .NET 8
- ASP.NET Core
- Docker
- MSSQL
- ReactJS
- Razor View
- Azure Functions
- SendPulse API key
- Tilda API key

## Installation

1. Clone the repository:

   ```bash
   git clone https://git.stxima.com/root/Stushbr.git
   cd Stushbr
   ```

2. Run the PaymentsGatewayWeb using Dockerfile:

   ```bash
   cd src/payments-gateway/Stushbr.PaymentsGatewayWeb
   docker build -t stushbr-payments-gateway .
   ```

3. Run the PaymentsGatewayWeb using Dockerfile:

   ```bash
   cd src/payments-gateway/Stushbr.EntityProcessor
   docker build -t stushbr-entity-processor .
   ```

4. For local setup without Docker:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

## Usage

1. Open your browser and go to: `http://localhost:5000`
2. To access the API, use the following endpoints:
   - `/api/payment` - for payment processing
   - `/api/entities` - for entity processing

## Testing

Currently, there are no tests implemented, but they are planned for future development.

## CI/CD

The project uses GitHub Actions for CI/CD automation. The main steps include:

1. Building the .NET project
2. Deploying the Azure Function
3. Deploying the web project
4. Building NodeJS

The GitHub Actions configuration can be found in `.github/workflows/`.

## Deployment

The project is deployed to Azure using Azure Functions for the backend.

## Contacts

For questions or suggestions, please contact me at Telegram: https://t.me/stxima.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
