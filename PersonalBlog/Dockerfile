FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["PersonalBlog/PersonalBlog.csproj", "PersonalBlog/"]
RUN dotnet restore "PersonalBlog/PersonalBlog.csproj"
COPY . .
WORKDIR "/src/PersonalBlog"
RUN dotnet build "PersonalBlog.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PersonalBlog.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV ASPNETCORE_ENVIRONMENT Production
CMD ASPNETCORE_URLS=http://*:$PORT dotnet PersonalBlog.dll