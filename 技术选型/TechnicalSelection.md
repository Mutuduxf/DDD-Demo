# 微服务技术选型

## 选型策略

1. 稳定大于性能
2. 经过市场考验
3. 开源/社区活跃
4. 避免技术绑定
5. 平台无关性
6. 便于横向扩展
7. 谨慎引入复杂性

## 选型类目

* CI/CD
  * 源码控制管理：Gitlab>Gitea>Gogs
  * 包管理器：ProGet/Harbor>NewGet
  * 持续集成：Jenkins>Drone？
  * 容器编排：云原生>Rancher>kubectl
* 服务治理
  * 服务注册/发现：Consul>Etcd>Zookeeper>Eureka
  * 配置中心：Consul>Etcd
  * API网关：Ocelot?
  * APM：Skywalking/Zipkin/Pinpoint
  * 日志监控：ELK/TIG
* 消息传递
  * 发布订阅：RabbitMQ>Kafka>NSQ>Puslar
  * 请求响应：Http/Grpc
* 持久化
  * RDB：SqlServer/Postgres>MySql>Oracle
  * DocumentDB：Mongo/CosmosDB
  * Key/Value：Redis>Memcached
* 支撑
  * 任务调度：Hangfire>Quartz.net