# TWQR Transaction Core

個人支付系統 POC／實作基礎，以公開 QR 支付情境與一般工程原則探索交易可靠性。

## 作品定位與本人貢獻

公開框架由 AI 協作建立。Larry 透過與 AI 討論提出需求、釐清規格與工程議題；此公開作品與公司正式 TWQR 系統分開呈現，不作為本人獨立手寫或完整維護所有功能的證明。

專案目前屬早期 POC。既有程式為實作基礎，規格中部分能力仍屬規劃；下列議題與目標不表示全部完成、已通過測試或具正式環境使用規模。完成範圍應以實際程式碼及驗證結果為準。

## 現有基礎與規格

目前以 C#／.NET 10 與分層專案骨架為基礎，程式與測試入口可由下列目錄查閱：

- [src](src/)
- [tests/TWQR.UnitTests](tests/TWQR.UnitTests/)
- [Payment Platform Specifications](docs/specs/README.md)
- [Delivery Roadmap](docs/specs/08-delivery-roadmap.md)

本介紹未對各功能完成度、建置結果、測試通過或效能作出額外承諾。

## 探索與規劃議題

- API 冪等性與重複請求防護。
- 交易狀態、未知外部結果及例外處理。
- 併發控制、重試與補償。
- Wallet／Ledger、結算及背景對帳。
- 依規格逐步推進模組與交付，相關架構方向以目前專案規格為準。

Redis、資料庫與部署工具等目標，應分別依規格、程式與可重現的驗證確認，不因列入設計而視為已完成。

## 使用邊界

本專案為個人學習與概念驗證作品，未經 FISC／財金或 iPASS／一卡通認可或背書。TWQR 名稱僅用於公開支付情境與公開標準脈絡，不包含公司內部程式碼、規格或交易資料，不處理真實金流。

## 作者

Larry Kang — C#／.NET 後端與電子支付工作經驗；公開 POC 以需求／規格討論及 AI 協作推進。

[GitHub](https://github.com/Larry-kang) · [LinkedIn](https://www.linkedin.com/in/larry-kang/)
