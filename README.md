# DDD-Demo

---

* 简介：此Demo用于展示DDD微服务的代码组织以及各层职责。
* 准备：出于git管理的考虑，优先使用文本格式的工具撰写脑图和UML/流程图/组织结构图等，请先准备好以下工具：
  * 脑图：后缀名km，请安装visual studio code插件vscode-mindmap。
  * UML/流程图/组织结构图等：后缀名drawio或者drawio.png，请安装visual studio code插件Draw.io Integration。

## 介绍

* 文件夹src中包含了MicroService以及BackendForFrontend两个模块，正常项目中模块是分开不同的代码仓库并有独立的CI/CD流程，此处出于方便展示的目的才放在一起。
* 此Demo只用于展示DDD微服务技术上的实践，不包含任何真实的业务逻辑，如有雷同纯属巧合。
* DDD是面向对象的方法论，它是一套开放的理论体系，重点在于遵守当中的原则，并因应现实情况（如项目的规模/要求、成本、团队大小、技术水平等）衍生出对应的实践与落地。此Demo只是展示了**其中**一种实践方式，而**不是唯一**正确的实践方式。因此请保持开放的心态，无需将此Demo当做唯一的标准答案。
* 出于关注点分离的考虑，每个层级的文件夹下均有对应的README以阐述此层/项目的职责以及设计原则。
* Relax~

## QuickStart

* 使用了Postgres作为数据库，可从[这里](https://hub.docker.com/_/postgres)运行一个Postgres容器进行测试。
* 此项目为CQS设计，C端（微服务）使用了Postgres主库，Q端（BackendForBrowser）使用了从库，出于方便调试的目的可将两者改为同一个库。

