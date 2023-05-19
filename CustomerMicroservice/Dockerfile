# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["CustomerMicroservice/CustomerMicroservice.csproj", "CustomerMicroservice/"]
RUN dotnet restore "CustomerMicroservice/CustomerMicroservice.csproj"
COPY . .
WORKDIR "/src/CustomerMicroservice"
RUN dotnet build "CustomerMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerMicroservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["CustomerMicroservice/distributed-programming-386414-26a5c9259d10.json", "/app"]
ENTRYPOINT ["dotnet", "CustomerMicroservice.dll"]
