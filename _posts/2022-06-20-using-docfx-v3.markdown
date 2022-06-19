---
layout: post
title:  "Using DocFX V3"
date:   2022-06-20 00:40:00 +1000
categories: categories
---

Over the past couple of days I have been experimenting with DocFX V3, and I decided on making a quick post on using it, as there doesn't seem to be much else out there on V3.

Please keep in mind DocFX V3 is still in beta, and things might change about it, if enough changes I will make a new post (if I feel like it is needed).

# Quick Questions

Just answer some quick question straight off the bat, if want skip the questions, go [here](#the-guide).

## DocFX V3?

Yes, DocFX V3 is a thing. It is built using .NET 6, and as such runs on all platforms natively without any issues.

## I thought DocFX V3 doesn't have API docs generation?

Well it does, but the pre-release builds the DocFX team provides lacks any of the API documentation generation, which in my opinion is kinda stupid, as one of the main features of DocFX is it's ability to generate these nice looking docs. However, it still lacks the ability to generate API documentation by only being provided a `.csproj` file, you MUST provide a pre-built `.dll` of your project with it's XML documentation. This is fine in my opinion, as if you just want automate this, then just create a script that calls `dotnet build -c Release`, then `docfx build`.

The API generating version is contained on the [v3-template](https://github.com/dotnet/docfx/tree/v3-template) branch.

# The Guide

## Prerequisites

```
Git
.NET 6
```

## Installing

Ok, so we need to install the new DocFX V3 beta, and as discussed in the previous section, the API generating version is contained on the [v3-template](https://github.com/dotnet/docfx/tree/v3-template) branch.

So clone the DocFX project, and switch to the v3-template branch:

```bash
git clone https://github.com/dotnet/docfx
cd docfx
git checkout v3-template
```

Once you have the DocFX project cloned to your local computer, build the entire solution. You can do this by either opening the `docfx.sln` in your IDE of choice, switching to `Release` mode and building or by running:

```bash
dotnet build -c Release docfx.sln
```

Once you have build the docfx solution, the executables (yes, there are multiple) will be placed at `src/docfx/bin/Release/net6.0/`.

Make sure that both `docfx` and `docfx-api-dotnet` executables are there (on Windows they will obviously have `.exe` extension).

You can now use the `docfx` executable to generate documentation, if you want to be able to use the executable globally, then either you will need to install it as a [.NET local tool](https://docs.microsoft.com/en-us/dotnet/core/tools/local-tools-how-to-use), or add it to your system's PATH ([Windows](https://js.educative.io/answers/how-to-add-an-application-path-to-system-environment-variables), [Linux](https://stackoverflow.com/questions/10235125/linux-custom-executable-globally-available)).

## Usage

To create a new DocFX site, do:

```bash
mkdir my-docfx-site && cd my-docfx-site
docfx new dotnet
```

This will setup a basic DocFX site, with an included example "CatLibrary" assembly that will have API documentation generated for. 

![Directory](/assets/blog/2022/06/20/using-docfx-v3/1-Directory.png)

To built the site, run:

```bash
docfx build
```

This will place the built site in `_site/`.

If we want to preview our built site, we will need a tool that can host static files on our local machine. One such tool is "[http-server](https://www.npmjs.com/package/http-server)". It is a Nodepackage, so it can easily be installed globally using npm (See the project's README for info on how-to). To get http-server to host our site locally, run:

```bash
http-server _site/
```

You will see what address http-server is hosting at in the console.

![http-server](/assets/blog/2022/06/20/using-docfx-v3/2-Http-Server.png)

If you open the address up in a browser, you will be able to see your generated site.

![Site Default](/assets/blog/2022/06/20/using-docfx-v3/3-Site-Default.png)

We can also see the generated API documentation for the provided "CatLibrary".

![Site API](/assets/blog/2022/06/20/using-docfx-v3/4-Site-API.png)

The default 'dotnet' template has two provided pages (the home page, and the getting-started page). Usually these pages are written using [DocFX flavoured markdown](https://docascode.github.io/template/spec/docfx_flavored_markdown/), but the home page is a custom `.yml` format. You can edit the `index.yml` file to see this, its very straight forward on what each key/value does. If you want to change the title then just edit `title`'s value, or if you want to change the description, just edit the `description`'s value.

```yml
#YamlMime:Home
hero:
  title: Voltstro is awesome!
  description: |
    Rowan SUXS
  actions:
  - name: Get Started
    href: getting-started.md
  - name: Install
    href: https://example.com

quickstart:
- title: Step 1
  content: |
    Install my tool using
    ```bash
    a bash script
    ```
- title: Step 2
  content: |
    Use the tool

highlights:
- title: Feature 1
  content: |
    ...
- title: Feature 2
  content: |
    ...
- title: Feature 3
  content: |
    ...
```

Once your done editing your site's pages, re-run the `docfx build` command, and locally host the site again for previewing.

![Site New Title](/assets/blog/2022/06/20/using-docfx-v3/5-Site-New-Title.png)

## Configuration

Configuring DocFX is done by editing the `docfx.json` file. By default, the configuration file looks like this:

```json
{
  "template": "https://github.com/docascode/template",
  "globalMetadata": {
    "_appTitle": "My Awesome Website",
    "_appFooter": "© Microsoft",
    "menu_path": "menu.yml"
  },
  "dotnet": {
    "assemblies": [
      "CatLibrary.dll"
    ]
  },
  "xref": [
    "https://docs.microsoft.com/en-us/dotnet/.xrefmap.json"
  ]
}
```

We can easily see that it has our site's title, the copyright that is displayed, as well as what assemblies to generate API documentation for. If you want to add more assemblies, then add another value with the location to it.

We can also see what other xref maps to link to. By default, the 'dotnet' template has just Microsoft's xref map.

This xref map specification has unfortunately changed since DocFX V2, as it is now all in `.json` format (instead of `.yml`), so we cannot link to other old DocFX V2 sites.

### Custom Themes

While DocFX V3 currently doesn't support themes in the sense that it just builds on top of the default theme, you can just modify the provided [template](https://github.com/docascode/template) to suit your needs. Just follow the README on the project on how to build.

I have already taken the project and modified for my needs by making it dark theme + some other adjustments.

If you want to use another template, change the `template` value in the site's configuration. If you want to use my modified template, change to:

```json
{
    "template": "https://gitlab.com/Voltstro-Studios/doctools/volt-docfxv3-theme-package",
    ...
}
```

Once you re-built the site, it will then look like:

![Dark Theme](/assets/blog/2022/06/20/using-docfx-v3/6-Site-Dark.png)
Ahh, much better, my eyes are no longer burning.

# Conclusion

So DocFX V3 is starting to look very good. Microsoft themselves already use DocFX V3 internally for their documentation. While it still has issues with it, and the slow pace of development has been a little bit disappointing, I myself will be using it for all future DocFX sites.
