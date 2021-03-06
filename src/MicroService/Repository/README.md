# 仓储层

---

## 构成

* 仓储实现
* 持久化对象（persistent object）

## 说明

* 仓储层用于实现领域层中的仓储接口（repository interface）。
* 仓储层与DAL是两种不同的设计思路，其区别在于：
  1. 只有聚合（Aggregate）才有仓储。
  2. 仓储是向上（领域层）服务，DAL是向下封装。
* 最简单的仓储层只有Add和Get两个方法，分别是添加聚合以及依据Id获取聚合，原因是：
  1. 聚合的Update应该使用状态跟踪功能来跟踪状态的变化，并在UOW提交的时候将新的状态持久化。
  2. Delete是一种面向数据的思维，业务系统中其实不应该是**删除**而是**作废**，作废也属于状态变化的一种，至于作废后是软/硬删除还是归档，由业务/技术需求来处理。
* 仓储的设计要依据命令查询分离（CQS）来处理，DDD只负责C端，所以仓储也只在C端，普通的查询（如列表页、报表等）应该在Q端的QueryService实现。
* 此处仓储实现使用了EF Core，假如要替换数据库的话只需要修改EntityConfigurations，repository本身不需要修改。
* 扩展：
  1. 此处展示的是通过ORM（EF）实现的仓储层，如果使用ADO.NET或者Dapper之类的Micro ORM，除了仓储层实现外还有persistent object（PO）。
  2. ORM实际上解决的是阻抗失配问题而不是数据库操作映射，因此不使用ORM时我们要手动解决阻抗失配，PO就是手段之一。
  3. 将无法直接持久化的聚合转换为依据RDB范式设计的PO，再使用PO进行持久化操作，在这里PO是依据持久化技术选型而进行的聚合的状态的映射。

