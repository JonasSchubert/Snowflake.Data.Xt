## [2.1.1](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v2.1.0...v2.1.1) (2025-05-07)


### Bug Fixes

* **deps:** :arrow_up: bump versions ([90e2f01](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/90e2f01fe58effe69db9b4d6d316706c85a62fdf))
* **sql:** :bug: replace double quotes with single quotes ([7bb0617](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/7bb0617df982d2d6fc8c6f8dd441f2dc2e1a717a))

# [2.1.0](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v2.0.1...v2.1.0) (2025-03-28)


### Features

* **commands:** add limit ([e1b9093](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/e1b9093d26429df658feb4f00f78c71d9676f1a2))
* **commands:** add limit ([f052ed8](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/f052ed84c11ba892d5ef23079e66c2cbe85bcbe6))
* **methods:** add SingleOrDefault and SingleOrDefaultAsync ([4244bce](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/4244bce49f5557b8c1423422f601f4325d7433a0))
* **methods:** add SingleOrDefault and SingleOrDefaultAsync ([51f6a25](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/51f6a25af5fa9b19a765f1ad7db9f7577f133f15))
* **order-by:** add additional methods for string usage ([a55bd24](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/a55bd246b67acbbab02d31ce4d2fe27632e7bf34))

## [2.0.1](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v2.0.0...v2.0.1) (2025-03-26)


### Bug Fixes

* **deps:** bump versions ([6669a16](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/6669a1609962505848ac9b718410399e96c222da))
* **order-by:** fix direction pattern matching check ([b3592a8](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/b3592a8344877dc60e00b496c72cf26aab4e09a0))
* **order-by:** validate direction ([17fd50b](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/17fd50b5a68d49b80d8983d6ddf1a115919495f8))

# [2.0.0](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.7.1...v2.0.0) (2024-12-09)


### Features

* **project:** add direct support for .net9.0, remove .net6.0 ([512c88a](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/512c88a604b42865e579f462bc2779650ac23e02))
* **where:** add support for string.Empty ([5465a22](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/5465a22253c8bd41c8c7bed8f8990f51e6126bd6))


### BREAKING CHANGES

* **project:** drop support for .net6.0

## [1.7.1](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.7.0...v1.7.1) (2024-04-09)


### Bug Fixes

* **deps:** bump Snowflake.Data from 3.0.0 to 3.1.0 ([#43](https://github.com/JonasSchubert/Snowflake.Data.Xt/issues/43)) ([b1f6371](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/b1f6371e4f2ae4e173bfff590d6697cf7f89ae4a))

# [1.7.0](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.6.0...v1.7.0) (2024-03-04)


### Features

* **snowflake-column-attribute:** allow to reuse same column names if alias is provided ([be044c1](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/be044c15abe7e5b9f40ca8c20905836e61cb7586))

# [1.6.0](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.5.1...v1.6.0) (2024-03-01)


### Features

* **command:** allow join of tables on itself ([d53d6f9](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/d53d6f9d61b6160b699c40ce9985f102ca0e5ac8))
* **nuget:** bump Snowflake.Data to v3.0.0 ([6cac371](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/6cac371fd3b665f3ba2fd158c7612b3368159c25))

## [1.5.1](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.5.0...v1.5.1) (2024-02-21)


### Bug Fixes

* **dependencies:** remove direct dependency to Meziantou.Analyzer ([d6bffee](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/d6bffee64d98b7ace36acbeebe35e378a3805c68))

# [1.5.0](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.4.0...v1.5.0) (2024-01-28)


### Bug Fixes

* **constructor:** add missing ctor accepting connection only ([ba85756](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/ba857562f024574150fa97feb9872d1842400189))


### Features

* **methods:** add overloads without parameter list for asyncs ([15e78ad](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/15e78ade051c4ed57e264d455e25ed3051ed6f29))

# [1.4.0](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.3.5...v1.4.0) (2024-01-26)


### Features

* **connection:** reuse one connection for better performance ([ce21000](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/ce210003598d6b886e25481825d2fcd0c8efaedd))

## [1.3.5](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.3.4...v1.3.5) (2024-01-15)


### Bug Fixes

* **column:** fallback to property name if not provided in attribute ([c12e00e](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/c12e00e690761bc27400f60b22e7c94116e8946f))

## [1.3.4](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.3.3...v1.3.4) (2024-01-12)


### Bug Fixes

* **attributes:** do not modify provided table or attribute name ([4d07c7f](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/4d07c7faa651176f9d86d9d3d2d0c425c37de2ea))

## [1.3.3](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.3.2...v1.3.3) (2023-12-27)


### Bug Fixes

* **where:** set correct parameter value ([690f183](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/690f18328ababd7f042c7ab718aa42b9af1b1173))

## [1.3.2](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.3.1...v1.3.2) (2023-12-25)


### Bug Fixes

* **deps:** bump Snowflake.Data from 2.1.4 to 2.1.5 ([#13](https://github.com/JonasSchubert/Snowflake.Data.Xt/issues/13)) ([d403ac7](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/d403ac7d0b1f56b21fdfc17f8b8fb40850293d68))

## [1.3.1](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.3.0...v1.3.1) (2023-12-15)


### Bug Fixes

* **project:** remove duplicated xml attribute ([da970d6](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/da970d6a78234af158181e434b9c25daf48351af))

# [1.3.0](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.2.0...v1.3.0) (2023-12-13)


### Features

* **where-clause:** reduce risk for injections ([1f1c0dd](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/1f1c0dd5e66e5be82a8daec62a0e3a496c8b55a6))

# [1.2.0](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.1.1...v1.2.0) (2023-12-13)


### Features

* **package:** use code analyzers to improve allover, add docs ([ceddb6c](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/ceddb6cad6124475a8a8f46ed79727279fecd5a2))

## [1.1.1](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.1.0...v1.1.1) (2023-12-06)


### Bug Fixes

* **async:** use ConfigureAwait(false) ([5719c5c](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/5719c5cc6601875cbc7d5063ecf8f0700be8d2a3))

# [1.1.0](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.0.2...v1.1.0) (2023-12-04)


### Bug Fixes

* **target-framework:** add net6.0 support to project files ([7099631](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/70996312198f6e2dce30cf22cea9f3512a32644e))
* **target-framework:** make net6.0 compatible ([47808dd](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/47808dd2f93ce4e3bacb02d07fe45b4d50d72be9))


### Features

* **target-framework:** add net6.0 support ([1da78eb](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/1da78ebab9e5049df0590725aa613ad16345a66c))

## [1.0.2](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.0.1...v1.0.2) (2023-12-04)


### Bug Fixes

* **where:** properly handle dynamic arguments ([6ea9059](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/6ea90597a74ddbaf5e3cd1ef25a6888b2c8bdede))

## [1.0.1](https://github.com/JonasSchubert/Snowflake.Data.Xt/compare/v1.0.0...v1.0.1) (2023-11-17)


### Bug Fixes

* **deps:** bump Snowflake.Data from 2.1.2 to 2.1.3 ([#2](https://github.com/JonasSchubert/Snowflake.Data.Xt/issues/2)) ([9807c60](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/9807c60da9f40721f060cb8627f0e21b27310ca8))

# 1.0.0 (2023-11-17)


### Features

* **project:** initial release ([b59e50b](https://github.com/JonasSchubert/Snowflake.Data.Xt/commit/b59e50b18b9a81e46dcdf4ad9de8dbf2219a4573))
