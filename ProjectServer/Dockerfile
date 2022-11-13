FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
# EXPOSE 5000
WORKDIR /app
# ENTRYPOINT ["dotnet", "RestApi.dll"]

# RUN apk add libgdiplus --update-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted && \
#     apk add terminus-font


FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build

WORKDIR /src

COPY ["ProjectServer.csproj","./"]
RUN dotnet restore "ProjectServer.csproj"
COPY . .
# WORKDIR "/src/src/grpcServer/"
RUN dotnet build "ProjectServer.csproj" -c Release -o /app/build

FROM build As publish
RUN dotnet publish "ProjectServer.csproj" -c Release -o /app/publish

FROM base As final
WORKDIR /app
COPY --from=publish /app/publish .
# RUN apk add --no-cache tzdata
ENV TZ Asia/Taipei
# ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "ProjectServer.dll"]
# RUN dotnet ef database update

#RUN dotnet ef database update

