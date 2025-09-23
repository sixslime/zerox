# Project Zerox
> A language to make games, a game made in said language, a server to host said game, and a client to connect to said server--all in pure C#.

Project Zerox is a loose designation for the following:
* A **DSL**, *FourZeroOne*, specialized for defining turn-based game logic. **[FourZeroOne](/src/SixShaded.FourZeroOne)**
* An **interface layer** (and specification) for *FourZeroOne* runtimes. **[FZO](/src/SixShaded.FourZeroOne/FZOSpec)**
* A minimal/default **implementation** of a *FourZeroOne* runtime. **[MinimaFZO](/src/SixShaded.MinimaFZO)**
* A **testing framework** for programs written in *FourZeroOne*. **[DeTes](/src/SixShaded.DeTes)**
* An implementation of **[401 Infinite](https://github.com/rtaylor034/401-infinite-paper)** written in *FourZeroOne*. **[Infinite](/SixShaded.FourZeroOne.Axois.Infinite)**
* A **server** program that runs said implementation of **[401 Infinite](https://github.com/rtaylor034/401-infinite-paper)** (to act as a source of truth between clients/players). ***TBD***
* A **protocol/API** for client programs to interact with said server program. ***TBD***
* A minimal/default **client** program that interacts with said server (acting as the frontend) through said protocol/API. ***TBD***

Completion of all of these components should result in a fully playable digital version of **[401 Infinite](https://github.com/rtaylor034/401-infinite-paper)**.

This repository serves as a monorepo/workspace for all things related to project Zerox while it is still in early development. Eventually, each part of project Zerox will be given it's own repository and project designation.

*Many names are temporary, better names will be chosen later in development.*
