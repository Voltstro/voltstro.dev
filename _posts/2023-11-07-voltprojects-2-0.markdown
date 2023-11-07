---
layout: post
title:  "VoltProjects 2.0"
date:   2023-11-07 22:00:00 +1000
categories: voltprojects
---

VoltProjects 2.0 is here. The long awaited release that no-one was waiting for.

## WTF is VoltProjects

Right, I never posted about VoltProjects at all. VoltProjects is an automatic documentation building and hosting service. It tracks a list of Git repos that have docs and builds their docs to then host the files.

I created this because I was sick of having multiple subdomains for each of my project's docs, and having to deploy them, either manually, or having a script through a CI to do it. And now I've spent two years of my life automating that... a task that takes less then five minutes.

## How does it Work?

VoltProjects has two applications, one called Server, and another one called Builder.

The server's job is to host the content publicly. Based off the requested project and the requested page will be found, then rendered and sent to client. Same stuff that every other major site has been doing since forever.

## The Details

Time to actually take a look into the major changes. Will talk a bit of nerd here to.

### Splitting the App

In VoltProjects 1.0, a single monolithic application was responsible for cloning git repos, building their docs and hosting the raw static files build out of them. There were a lot of issues with this design, the main one being crashing, and forcing re-builds. If the app crashed, it took the server hosting the content down with it too, and if you wanted to force a re-build of everything, once again, you had to take down the server.

So I split the application into two. One called Builder and one called Server. I already explain what app does what above.

### App Development Decisions

Both apps are still built with .NET, with the server being ASP.NET Core, and the builder using [Microsoft.Extensions.Hosting](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host).

However, while developing this version I had a play-around with having the front-end being an SPA, instead of server-side rendered with MVC. I tried React, Vue and Angular. Going the SPA route does have the advantage of really nice interactivity, and just having a server that focuses on being an API, but everything else kinda takes a hit in someway. The first is end-user performance; The end user has to sit around and download 5mb+ of Javascript just to be able to view the docs, yes their are "meta-frameworks" (you will get use to the bullshit names JS devs give everything) such as NextJs and Nuxt, but these things kinda personally suck to use. Everything has to be "their" way, and I don't like "their" way.

Another thing is that this is documentation, the nice page transitions that an SPA provided to different pages are nice, but is not needed.

### A Database

We got technology straight from the 1970s finally. Projects and their pages are stored in a Database. Previously everything was stored on disk, and the server just directly hosted the static content. This was honestly fine, but if you want to a standard template, with standard elements (such as the shared VP navbar) it becomes a bit more of a problem. Early in development, I made the build process spit out JSON files, then have the server read and parse the JSON files and render out the content to the client. Problem with this is that its slow, so instead I made the Builder still spit out JSON files, but then read them and put them into a database in a standard format that the server can then just simply read from.

For the DBMS, I choose Postgres. Now maybe Postgres is overkill, but I'm a lot more familiar with it, plus it means I don't ever have to worry about "hey guys we moving from *x* to Postgres"  issue.

### General Infrastructure Updates

The last major change I will talk about is infrastructure updates, the stuff that no one will ever see. I self-host the majority of my stuff, running it all on my home server. The previous version was ran directly on a Linux VM, with no Docker, using systemd to start/restart the app. It was shit.

But now I'm using Kubernetes. Now I will say, setting Kubernetes up was a bit of a pain in the ass, but it makes managing and deploying so much easier. No more sshing into the VM, copying the new version's files to it, and restarting the systemd service. Now it's just a "simple" yml file, and `kubectl apply -f voltprojects.yaml`.

Since we using Kubernetes, the apps get built into a Docker image, and are hosted on [Docker Hub](https://hub.docker.com/repositories/voltstro).

## Links

- [GitHub Repo](https://github.com/Voltstro/VoltProjects)
- [VP Server Docker](https://hub.docker.com/repository/docker/voltstro/vpserver)
- [VP Builder Docker](https://hub.docker.com/repository/docker/voltstro/vpbuilder)
