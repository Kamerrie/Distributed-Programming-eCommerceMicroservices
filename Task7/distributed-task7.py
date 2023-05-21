from flask import Flask, request, jsonify
import requests
import os

app = Flask(__name__)

@app.route('/change_shipping_status', methods=['POST'])
def change_shipping_status():
    # Obtain the required information from the request
    data = request.json
    shipping_id = data.get('shippingId')
    shipping_status = data.get('shippingStatus')
    customer_id = data.get('customerId')
    
    # Update the shipping status
    shipping_update_response = requests.put(
        f'https://shipping-service-a3rfqhor5q-no.a.run.app/api/Shipping/{shipping_id}',
        headers={'Content-Type': 'application/json'},
        json={'shippingStatus': shipping_status}
    )
    
    # Check if the shipping status was successfully updated
    if shipping_update_response.status_code != 204:
        return jsonify({'message': 'Failed to update shipping status'}), 500
    
    # Notify the customer about the change in shipping status
    notification_response = requests.post(
        f'https://customer-service-a3rfqhor5q-no.a.run.app/api/Customer/users/{customer_id}/notifications',
        headers={'Content-Type': 'application/json'},
        json={'message': f'Your order shipping status is now: {shipping_status}'}
    )
    
    # Check if the notification was successfully sent
    if notification_response.status_code != 200:
        return jsonify({'message': 'Failed to send notification to customer'}), 500
    
    # Return a success response
    return jsonify({'message': 'Shipping status updated and notification sent to customer'}), 200


if __name__ == '__main__':
    port = int(os.environ.get("PORT", 8080))
    app.run(host='0.0.0.0', port=port)
