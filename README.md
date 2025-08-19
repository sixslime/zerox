# Project Zerox
> A language to make games, a game made in said language, a server to host said game, and a client to connect to said server--all in pure C#.

Project Zerox is a loose designation for the following:
* A **DSL** that defines game logic. **[FourZeroOne](/src/SixShaded.FourZeroOne)**
* An **API** that provides an abstract interface layer for--but does not implement--the **FZO** runtime. **[FZO](/src/SixShaded.FourZeroOne/FZOSpec)**
* A minimal/default **implementation** of the **FZO** runtime. **[MinimaFZO](/src/SixShaded.MinimaFZO)**
* A **testing framework** for programs written in **FourZeroOne**. **[DeTes](/src/SixShaded.DeTes)**
* An implementation of **[401 Infinite](https://github.com/rtaylor034/401-infinite-paper)** written in FZO. **[Infinite](/SixShaded.FourZeroOne.Axois.Infinite)**
* A **server** program that runs said implementation of 401 Infinite (acting as the backend) and exposes endpoints for clients to interact with it. ***TBD***
* A **protocol/API** for client programs to interact with said server program. ***TBD***
* A minimal/default **client** program that interacts with said server (acting as the frontend) through said protocol/API. ***TBD***

Theoretically, after all of these sub-projects have been created, a full 

This repository serves as a monorepo/workspace for all things related to project Zerox while it is still in early development. Eventually, each part of project Zerox will be given it's own repository and project designation.

*Many names are temporary, better names will be chosen later in development!*
