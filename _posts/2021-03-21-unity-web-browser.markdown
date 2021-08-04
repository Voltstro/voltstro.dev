---
layout: post
title:  "Unity Web Browser"
date:   2021-03-21 00:05:00 +1000
hero_image: "/assets/blog/2021/03/21/unity-web-browser/Banner.jpg"
hero_height: 380px
---

Recently, I wanted to have a web to be displayed within my game. Ok, no problem, I&#39;ll get a web browser engine, get it to render to an image and boom and I will be done.

But nope, it wasn&#39;t that easy of course. So, I set out to sail the seas of the internet to find or develop a solution.

I had a few requirements for my embedded browser:

- It needs to be cross-platform
- Up to date
- Support modern day HTML5 and CSS
- Open-Source
- Be used for displaying the web, not as an UI framework

## Initial attempts

At first, I thought I could just use the [CefSharp](https://github.com/cefsharp/CefSharp) project in Unity and use [CEF](https://bitbucket.org/chromiumembedded/cef). But the problem with CefSharp is that its Windows only, and my game will be supporting Windows and Linux, so it&#39;s a no go for that.

Ok, well what other options are there then? Well, there is another CEF wrapper called [CefGlue](https://gitlab.com/xiliumhq/chromiumembedded/cefglue), and it targets mono with Windows and Linux, so thats perfect. But once again, there is another issue, as shown by this [project](https://github.com/aleab/cef-unity-sample), CEF with CefGlue does work in Unity, but it crashes when you enter play mode for the second time. This is due to how Unity handles native plugins, and the way that CEF was programmed.

Once Unity loads a native plugin, it never unloads it, and once CEF loads, it never let goes of it external files (such as cef.pak), so what was happening was CEF was loading external files, but never unloading them, so it would go to load them again, but they were already in use, causing Unity to crash.

### Ultralight

Another is [Ultralight](https://ultralig.ht/), a modern-day web browser engine, made by the same people behind the now defunct Awesomium project, and its significantly lighter then CEF. But even though it advertises itself to be able to embed in games, it currently suffers the same issues with CEF… bruh. Hopefully in the future they can flush out this problem. Ultralight also has some weird licensing, with a large chunk of the code closed-sourced and under a proprietary license.

### Servo

[Servo](https://servo.org/) is a web browser engine that was first developed by Mozllia and is now in the hands of the Linux Foundation. Its goals is to be a modern day, cross-platform engine written in the Rust programming language. It&#39;s also open-source under the MPL-2.0 license. However, like the others, it&#39;s got problems. Its major problem is that it&#39;s still an experimental browser, ok that&#39;s fine for the most part, half the software I use anyway is beta or experimental software, but the big question is, does it work in Unity???? Well yea, supposably. This [project](https://github.com/MozillaReality/servo-unity) I found uses it in Unity, but its MacOS only, yuck. However supposably it should work with Windows and Linux, Servo itself is cross-platform.

&quot;Ok, so fix it and compile it for yourself retard&quot;, that might be what you are saying, but I have no fucking clue on how to use or compile Rust.

In the future, if I am bothered enough to understand the basics of Rust, then maybe this might be a good contender to replace the browser engine with.

# So, what did you do?

Well, there is one way, and that&#39;s to render the browser in a separate process and send data between the two processes.

So that&#39;s exactly what I did. I programmed a process in .NET 5 that uses CefGlue and CEF to render the browser, and used a ZeroMQ to communicate between that process and Unity. And with .NET 5&#39;s ability to [compile into a single application](https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file), it comes as a single ~30mb file (not including the external CEF libs).

After a few weeks slaving over Unity and Visual Studio, I finally got it to display a website. I also added the ability to handle keyboard and mouse events and use multi-threading for communication.

I made the entire thing open source under the LGPL-3.0 license; you can find the project [here](https://github.com/Voltstro-Studios/UnityWebBrowser).

## Using it in your own project

I designed the project to use a Unity package, so you can install it with the Unity package manager with this git URL:

```
https://gitlab.com/Voltstro-Studios/WebBrowser/Package.git
```

I am still flushing out some minor issues, so if you find an issue, please report it on the repo.

## Screenshots

Here are some screenshots of it working in the editor and as a player build.

![Screenshot 1](/assets/blog/2021/03/21/unity-web-browser/Screenshot-Editor1.png)

![Screenshot 2](/assets/blog/2021/03/21/unity-web-browser/Screenshot-Editor2.png)

![Screenshot 3](/assets/blog/2021/03/21/unity-web-browser/Screenshot-Editor3.png)

![Screenshot 4](/assets/blog/2021/03/21/unity-web-browser/Screenshot-InPlayer.png)
