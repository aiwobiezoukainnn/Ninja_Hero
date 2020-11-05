 #import "IAPInterface.h"
 #import "IAPManager.h"

 @implementation IAPInterface
 IAPManager *iapManager = nil;

 void InitIAPManager(){
     iapManager = [[IAPManager alloc] init];
     [iapManager attachObserver];     
 }

 bool IsProductAvailable(){
     return [iapManager canMakePayment];
 }

 void RealBuyProduct(void *p){
     [iapManager buyRequest:[NSString stringWithUTF8String:p]];
 }
 @end