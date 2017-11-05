FROM ubuntu:16.04 
MAINTAINER Daisuke Yamamura(mucho613) <mucho613@gmail.com>

# Specify API keys
ARG GOOGLE_KEY=
ARG GOOGLE_CX=
ARG YAHOO_KEY=
ARG TWITTER_CONSUMER_KEY=
ARG TWITTER_CONSUMER_SECRET=
ARG TWITTER_ACCESS_TOKEN=
ARG TWITTER_ACCESS_TOKEN_SECRET=

# Prepare to clone repository
RUN apt update
RUN apt install -y git

# Install Mono
RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
RUN echo "deb http://download.mono-project.com/repo/ubuntu xenial main" | tee /etc/apt/sources.list.d/mono-official.list
RUN apt update
RUN apt install -y mono-devel

# Install JRE, JDK
RUN apt install -y default-jre 
RUN apt install -y default-jdk

# Set locale
RUN apt install -y locales
RUN locale-gen en_US.UTF-8  
ENV LANG en_US.UTF-8  
ENV LANGUAGE en_US:en  
ENV LC_ALL en_US.UTF-8

# Git clone and prepare to build
RUN git clone https://github.com/gedorinku/nagominashare-bot
RUN cp -r nagominashare-bot/generator/Assets nagominashare-bot/bot/Assets

# Set API keys
WORKDIR /nagominashare-bot/generator
RUN cp Variables.cs.sample Variables.cs
RUN sed -i -e "s/\"your api key\"/\"$GOOGLE_KEY\"/g" Variables.cs
RUN sed -i -e "s/\"cx\"/\"$GOOGLE_CX\"/g" Variables.cs
RUN sed -i -e "s/\"your app id\"/\"$YAHOO_KEY\"/g" Variables.cs

WORKDIR /nagominashare-bot/bot
RUN cp twitter4j.properties.sample twitter4j.properties
RUN sed -i -e "s/true/false/g" twitter4j.properties
RUN sed -i -e "/oauth.consumerKey=/D" twitter4j.properties
RUN sed -i -e "/oauth.consumerSecret=/D" twitter4j.properties
RUN sed -i -e "/oauth.accessToken=/D" twitter4j.properties
RUN sed -i -e "/oauth.accessTokenSecret=/D" twitter4j.properties
RUN echo oauth.consumerKey=$TWITTER_CONSUMER_KEY >> twitter4j.properties
RUN echo oauth.consumerSecret=$TWITTER_CONSUMER_SECRET >> twitter4j.properties
RUN echo oauth.accessToken=$TWITTER_ACCESS_TOKEN >> twitter4j.properties
RUN echo oauth.accessTokenSecret=$TWITTER_ACCESS_TOKEN_SECRET >> twitter4j.properties
RUN cp bot.properties.sample bot.properties

# Build generator
WORKDIR /nagominashare-bot/generator
RUN xbuild
RUN mv bin/Debug/generator.exe ../bot/generator.exe

# Run bot
CMD cd /nagominashare-bot/bot && \
    ./gradlew run
