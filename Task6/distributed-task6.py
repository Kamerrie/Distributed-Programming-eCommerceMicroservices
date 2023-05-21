from flask import Flask, request
import requests
import json
import os

app = Flask(__name__)

@app.route('/confirm_order', methods=['POST'])
def confirm_order():
    data = request.get_json()

    # Extract data from request
    customerId = data['customerId']
    products = data['products']
    paymentDetails = data['paymentDetails']
    shippingDetails = data['shippingDetails']

    # Create the order
    order_url = f'https://order-service-a3rfqhor5q-no.a.run.app/api/Orders?customerId={customerId}'
    order_response = requests.post(order_url, json=products)

    
    order_dict = json.loads(order_response.text)
    order_id = order_dict['orderId']
    order_amount = order_dict['totalPrice']

    # Log the payment
    payment_url = 'https://payment-service-a3rfqhor5q-no.a.run.app/api/Payment'
    paymentDetails["orderId"] = order_id
    paymentDetails["amount"] = order_amount
    paymentDetails["customerId"] = customerId
    payment_response = requests.post(payment_url, json=paymentDetails)

    payment_dict = json.loads(payment_response.text)
    payment_id = payment_dict['paymentId']

    # Record the shipping details
    shipping_url = 'https://shipping-service-a3rfqhor5q-no.a.run.app/api/Shipping'
    shippingDetails["orderId"] = order_id
    shippingDetails["customerId"] = customerId
    shippingDetails["paymentId"] = payment_id
    shipping_response = requests.post(shipping_url, json=shippingDetails)

    # Notify the customer
    notification_url = f'https://customer-service-a3rfqhor5q-no.a.run.app/api/Customer/users/{customerId}/notifications'
    
    notification_data = {"message": f"order id: {order_id} received, not yet dispatched."}
    notification_response = requests.post(notification_url, json=notification_data)

    return f'Order {order_id} Confirmed and Created', 200


if __name__ == '__main__':
    port = int(os.environ.get("PORT", 8080))
    app.run(host='0.0.0.0', port=port)