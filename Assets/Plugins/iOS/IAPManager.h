#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>
 @interface IAPManager : NSObject<SKProductsRequestDelegate, SKPaymentTransactionObserver>{
     SKProduct *proUpgradeProduct;
     SKProductsRequest *productsRequest;
     NSString *currentProId;
     
 }
  //@property (nonatomic,copy) NSString *currentProId;
 -(void)attachObserver;
 -(BOOL)canMakePayment;
 -(void)requestProductData:(NSString *)productIdentifiers;
 -(void)buyRequest:(NSString *)productIdentifier;
 @end
