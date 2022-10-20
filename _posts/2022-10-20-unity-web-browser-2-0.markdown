---
layout: post
title:  "Unity Web Browser 2.0"
date:   2022-10-20 16:00:00 +1000
categories: unity
---

The second major version of Unity Web Browser (UWB) is now here, with
enormous amounts of changes and improvements done to it.

So let’s go through some major changes done to it.

# The (Main) Goodies

## Proper Docs

One thing that UWB 1.x was missing was any type of documentation (other then the README). The project now has [it's own section](https://projects.voltstro.dev/UnityWebBrowser/) on VoltProjects. These docs are built from the `docs/` directory that is in the repo.

## VoltRpc

The 1.x version of UWB used ZeroMQ for handling communications. 2.0 now uses VoltRpc for communications. If you want to know more about VoltRpc, you can read the [blog post I made](https://voltstro.dev/blog/2021/08/03/introducing-voltrpc) when I initially released it.

Using VoltRpc means doing communications between the engine and the core is a lot easier to program.

## Abstracted Engines

UWB 1.x was originally designed to use CEF, and that’s it. UWB 2.0’s layer between the engine and the core has now been abstracted. The core will no longer will care about what web engine the engine process is using, and has been coded in such a way (The API as well). The only thing the core does care about is the abstraction layer.

## Better UNIX Support

UWB 2.0 features better UNIX support. Support for Linux did already exist, but had some issues with permissions among other things, but these issues should hopefully be solved.

MacOS support still hasn’t been done. More details about this will be down below.

## Expanded API

UWB’s core API has been expanded quite a lot.

The new input handling system makes using different input systems (such as the legacy input vs. the new input system) a lot easier. The input handler uses scriptable objects, which allows for easy changing of input handlers, as well as custom input systems (if they exist?) to easily be added.

Communication layers are a thing now too, allowing different communication protocols to be added. Be default TCP is provided and used. Pipes can be used if desired, and is provided by a separate package.

An API for popups has also been added (since this was requested). Popups can be easily be controlled by capturing them with events. Please note that popups are disabled by default, but can easily be changed by changing the popup action property on the client.

UWB 2.0’s API is completely breaking from 1.x, and will likely require a large amount of rewriting (if you heavily use its API).

## Proper Installation

No longer will you have to manually install UWB by adding a bunch of different GIT packages hosted on GitLab. UWB 2.x will be provided a proper custom registry. The registry is hosted and maintained by me(Voltstro). For more info on it, see [VoltstroUPM](https://github.com/Voltstro/VoltstroUPM).

As much as we would love to provide the packages via public, readily available custom registries (such as OpenUPM), the CEF binaries are too large to be hosted by them (technically it’s a limit with GitHub, since OpenUPM just pulls from there).

## A lot more stuff

There actually a lot more stuff; so much that I can’t remember it all. The development of the “update” was done over a year (on and off).

# The Future?

Plans for 2.1 are already under-way. Please note that the items listed may or may not be done, there just plans at the moment.
  - Some MacOS stuff has already been done (it was originally planned
    for 2.0), but a lot more work will need to be done (due to the
    stupid custom way chromium apps need to be packaged on MacOS).
  - Changes to where the cache is stored will most likely be done as
    well.
  - I also want to introduce some new engine(s), mainly one using
    Microsoft’s WebView, but some more research will need to done.

However I do plan on taking a bit of break from working on UWB, as I want to work on some other projects for a bit.

# Start Using

To start using UnityWebBrowser 2.0, checkout it’s [project site](https://projects.voltstro.dev/UnityWebBrowser/)!

It's [GitHub can still be found](https://github.com/Voltstro-Studios/UnityWebBrowser) were it always has been. There is also a [GitLab mirror](https://gitlab.com/Voltstro-Studios/WebBrowser/UnityWebBrowser).

# The Best The World Had To Offer

I wanted to apologies on how long this release took, for a large chunk of time the release has basically been ready, it’s just that I been busy too busy with life stuff to actually create the release. But hey, its here now, enjoy.

Anyway, that’s it for this post; I’m gonna go sleep or something now.
