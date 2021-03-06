﻿// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreAssetsInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class AssetStoreAssetsInfo : AssetStoreResultBase<AssetStoreAssetsInfo>
  {
    internal Dictionary<int, AssetStoreAsset> assets = new Dictionary<int, AssetStoreAsset>();
    internal AssetStoreAssetsInfo.Status status;
    internal bool paymentTokenAvailable;
    internal string paymentMethodCard;
    internal string paymentMethodExpire;
    internal float price;
    internal float vat;
    internal string currency;
    internal string priceText;
    internal string vatText;
    internal string message;

    internal AssetStoreAssetsInfo(AssetStoreResultBase<AssetStoreAssetsInfo>.Callback c, List<AssetStoreAsset> assets)
      : base(c)
    {
      using (List<AssetStoreAsset>.Enumerator enumerator = assets.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AssetStoreAsset current = enumerator.Current;
          this.assets[current.id] = current;
        }
      }
    }

    protected override void Parse(Dictionary<string, JSONValue> dict)
    {
      Dictionary<string, JSONValue> dictionary = dict["purchase_info"].AsDict(true);
      string str = dictionary["status"].AsString(true);
      if (str == "basket-not-empty")
        this.status = AssetStoreAssetsInfo.Status.BasketNotEmpty;
      else if (str == "service-disabled")
        this.status = AssetStoreAssetsInfo.Status.ServiceDisabled;
      else if (str == "user-anonymous")
        this.status = AssetStoreAssetsInfo.Status.AnonymousUser;
      else if (str == "ok")
        this.status = AssetStoreAssetsInfo.Status.Ok;
      this.paymentTokenAvailable = dictionary["payment_token_available"].AsBool();
      if (dictionary.ContainsKey("payment_method_card"))
        this.paymentMethodCard = dictionary["payment_method_card"].AsString(true);
      if (dictionary.ContainsKey("payment_method_expire"))
        this.paymentMethodExpire = dictionary["payment_method_expire"].AsString(true);
      this.price = dictionary["price"].AsFloat(true);
      this.vat = dictionary["vat"].AsFloat(true);
      this.priceText = dictionary["price_text"].AsString(true);
      this.vatText = dictionary["vat_text"].AsString(true);
      this.currency = dictionary["currency"].AsString(true);
      this.message = !dictionary.ContainsKey("message") ? (string) null : dictionary["message"].AsString(true);
      using (List<JSONValue>.Enumerator enumerator = dict["results"].AsList(true).GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          JSONValue current = enumerator.Current;
          AssetStoreAsset assetStoreAsset;
          if (this.assets.TryGetValue(!current["id"].IsString() ? (int) current["id"].AsFloat() : int.Parse(current["id"].AsString()), out assetStoreAsset))
          {
            if (assetStoreAsset.previewInfo == null)
              assetStoreAsset.previewInfo = new AssetStoreAsset.PreviewInfo();
            AssetStoreAsset.PreviewInfo previewInfo = assetStoreAsset.previewInfo;
            assetStoreAsset.className = current["class_names"].AsString(true).Trim();
            previewInfo.packageName = current["package_name"].AsString(true).Trim();
            previewInfo.packageShortUrl = current["short_url"].AsString(true).Trim();
            assetStoreAsset.price = !current.ContainsKey("price_text") ? (string) null : current["price_text"].AsString(true).Trim();
            previewInfo.packageSize = int.Parse(!current.Get("package_size").IsNull() ? current["package_size"].AsString(true) : "-1");
            assetStoreAsset.packageID = int.Parse(current["package_id"].AsString());
            previewInfo.packageVersion = current["package_version"].AsString();
            previewInfo.packageRating = int.Parse(current.Get("rating").IsNull() || current["rating"].AsString(true).Length == 0 ? "-1" : current["rating"].AsString(true));
            previewInfo.packageAssetCount = int.Parse(!current["package_asset_count"].IsNull() ? current["package_asset_count"].AsString(true) : "-1");
            previewInfo.isPurchased = current.ContainsKey("purchased") && current["purchased"].AsBool(true);
            previewInfo.isDownloadable = previewInfo.isPurchased || assetStoreAsset.price == null;
            previewInfo.publisherName = current["publisher_name"].AsString(true).Trim();
            previewInfo.packageUrl = !current.Get("package_url").IsNull() ? current["package_url"].AsString(true) : string.Empty;
            previewInfo.encryptionKey = !current.Get("encryption_key").IsNull() ? current["encryption_key"].AsString(true) : string.Empty;
            previewInfo.categoryName = !current.Get("category_name").IsNull() ? current["category_name"].AsString(true) : string.Empty;
            previewInfo.buildProgress = -1f;
            previewInfo.downloadProgress = -1f;
          }
        }
      }
    }

    internal enum Status
    {
      BasketNotEmpty,
      ServiceDisabled,
      AnonymousUser,
      Ok,
    }
  }
}
