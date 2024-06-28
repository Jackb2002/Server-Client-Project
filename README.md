# Chat Client

This project demonstrates a simple chat client application in C# using Windows Forms, TCP/IP sockets for communication, RSA encryption for secure messaging, and JSON serialization for message formatting.

## Features

- **Client Connectivity**: Connects to a server using TCP/IP sockets.
- **Secure Communication**: Implements RSA encryption for message security.
- **Message Types**: Supports different message types (e.g., text, image, file).
- **JSON Serialization**: Converts messages to JSON format for transmission.

## Getting Started

### Prerequisites

- Visual Studio (2019 or later) with .NET Core SDK
- Newtonsoft.Json package for JSON serialization

### Installation

1. Clone the repository:

git clone https://github.com/Jackb2002/Server-Client-Project.git

2. Open the solution file `Chatter_Project.sln` in Visual Studio.

3. Restore NuGet packages if necessary.

4. Build and run the `ChatClient` project or `ChatServer` project.

## Usage

1. Enter the server IP address and port in the client application.
2. Click Connect to establish a connection to the server.
3. Enter a message in the input box and click Send to transmit the message to the server.

## Project Structure

- `ChatClient`: Contains the Windows Forms application.
- `MainForm.cs`: Main form with UI controls and event handlers.
- `Client.cs`: Client class handling server connection, message sending, and encryption.
- `Message.cs`: Class defining the structure of a server-client message and JSON serialization/deserialization.

## Dependencies

- **Newtonsoft.Json**: Used for JSON serialization and deserialization.


