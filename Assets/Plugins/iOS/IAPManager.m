#import "IAPManager.h"
@implementation IAPManager


 -(void) attachObserver{

     NSLog(@"AttachObserver");
     [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
 }

 -(BOOL) CanMakePayment{
     return [SKPaymentQueue canMakePayments];
 }

//去苹果服务器请求商品
- (void)requestProductData:(NSString *)productId{
    NSLog(@"-------------请求对应的产品信息----------------");
    NSArray *product = [[NSArray alloc] initWithObjects:productId,nil];
    NSSet *nsset = [NSSet setWithArray:product];
    SKProductsRequest *request = [[SKProductsRequest alloc] initWithProductIdentifiers:nsset];
    request.delegate = self;
    [request start];
}

//收到产品返回信息
- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response{

    if (request == nil || response == nil ) {
        return;
    }
    NSLog(@"--------------收到产品反馈消息---------------------");
    NSArray *product = response.products;
    if([product count] == 0){
        NSLog(@"--------------没有商品------------------");
        return;
    }

    NSLog(@"productID:%@", response.invalidProductIdentifiers);
    NSLog(@"产品付费数量:%lu",(unsigned long)[product count]);

    SKProduct *p = nil;
    for (SKProduct *pro in product) {
        NSLog(@"%@", [pro description]);
        NSLog(@"%@", [pro localizedTitle]);
        NSLog(@"%@", [pro localizedDescription]);
        NSLog(@"%@", [pro price]);
        NSLog(@"%@", [pro productIdentifier]);

        if([pro.productIdentifier isEqualToString:currentProId]){
            p = pro;
        }
    }

    SKPayment *payment = [SKPayment paymentWithProduct:p];
    NSLog(@"发送购买请求");
    [[SKPaymentQueue defaultQueue] addPayment:payment];
}

//请求失败
- (void)request:(SKRequest *)request didFailWithError:(NSError *)error{
    NSLog(@"------------------错误-----------------:%@", error);
}

- (void)requestDidFinish:(SKRequest *)request{
    NSLog(@"------------反馈信息结束-----------------");
}

 -(void)buyRequest:(NSString *)productIdentifier{
	 currentProId = productIdentifier;
     [self requestProductData:currentProId];
 }


 -(NSString *)transactionInfo:(SKPaymentTransaction *)transaction{     
     return [self encode:(uint8_t *)transaction.transactionReceipt.bytes length:transaction.transactionReceipt.length];
 }
 
 -(NSString *)encode:(const uint8_t *)input length:(NSInteger) length{
     static char table[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
     
     NSMutableData *data = [NSMutableData dataWithLength:((length+2)/3)*4];
     uint8_t *output = (uint8_t *)data.mutableBytes;
     
     for(NSInteger i=0; i<length; i+=3){
         NSInteger value = 0;
         for (NSInteger j= i; j<(i+3); j++) {
             value<<=8;
             
             if(j<length){
                 value |=(0xff & input[j]);
             }
         }         
         NSInteger index = (i/3)*4;
         output[index + 0] = table[(value>>18) & 0x3f];
         output[index + 1] = table[(value>>12) & 0x3f];
         output[index + 2] = (i+1)<length ? table[(value>>6) & 0x3f] : '=';
         output[index + 3] = (i+2)<length ? table[(value>>0) & 0x3f] : '=';
     }     
     return [[NSString alloc] initWithData:data encoding:NSASCIIStringEncoding];
 }

 -(void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions{
     for (SKPaymentTransaction *transaction in transactions) {
         switch (transaction.transactionState) {
             case SKPaymentTransactionStatePurchased:
                 [self completeTransaction:transaction];
                 break;
             case SKPaymentTransactionStateFailed:
                 [self failedTransaction:transaction];
                 break;
             case SKPaymentTransactionStateRestored:
                 [self restoreTransaction:transaction];
                 break;
             default:
                 break;
         }
     }
 }


 -(void) completeTransaction:(SKPaymentTransaction *)transaction{
     NSLog(@"Comblete transaction : %@",transaction.transactionIdentifier);
     UnitySendMessage("Shop", "onSuccess", [[self transactionInfo:transaction] UTF8String]);
     [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
 }


 -(void) failedTransaction:(SKPaymentTransaction *)transaction{
     NSLog(@"Failed transaction : %@",transaction.transactionIdentifier);
     UnitySendMessage("Shop", "onFailed", [[self transactionInfo:transaction] UTF8String]);
     if (transaction.error.code != SKErrorPaymentCancelled) {
         NSLog(@"!Cancelled");
     }
     [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
 }


 -(void) restoreTransaction:(SKPaymentTransaction *)transaction{
     NSLog(@"Restore transaction : %@",transaction.transactionIdentifier);
     [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
 }
 @end

