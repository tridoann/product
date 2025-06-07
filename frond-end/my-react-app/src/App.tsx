import '@ant-design/v5-patch-for-react-19';
import React, { useState } from "react";
import { Form, Input, InputNumber, Button, notification } from "antd";
import "antd/dist/reset.css";
import { apiClient } from "./common";

interface Product {
  name: string;
  price: number;
  description: string;
}

const UploadForm: React.FC = () => {
  const [form] = Form.useForm();
  const [fileList, setFileList] = useState<any[]>([]);

  const onFinish = async (values: any) => {
    const { name, price, description } = values;
    const productRequest: Product = {
      name,
      price: parseFloat(price),
      description,
    };
    try {
      const response = await apiClient.post("api/products", productRequest);
      if (response.status >= 200 && response.status < 300) {
        form.resetFields();
        setFileList([]);
        notification.success({
          message: "Success",
          description: `Request successfully!, at path: ${response.data.id}`,
        });
      } else {
        notification.error({
          message: "Error",
          description: `${response.data.title}, status: ${response.status}`,
        });
      }
    } catch (error: any) {
      notification.error({
        message: "Error",
        description: "An error occurred during request.",
      });
    }
  };

  return (
    <div style={{ 
      display: "flex", 
      justifyContent: "center", 
      alignItems: "center", 
      height: "100vh"
    }}>
      <div style={{ 
        maxWidth: 400, 
        width: "100%", 
        padding: "20px", 
        background: "#fff", 
        borderRadius: "8px", 
        boxShadow: "0 2px 10px rgba(0,0,0,0.1)" 
      }}>
        <Form form={form} onFinish={onFinish} layout="vertical">
          <Form.Item
            label="Name"
            name="name"
            rules={[{ required: true, message: "Please input the product name!" }]}
          >
            <Input placeholder="Enter product name" />
          </Form.Item>
          <Form.Item
            label="Description"
            name="description"
            rules={[{ required: true, message: "Please input the product description!" }]}
          >
            <Input placeholder="Enter product description" />
          </Form.Item>
          <Form.Item
            label="Price"
            name="price"
            rules={[{ required: true, message: "Please input the price!" }]}
          >
            <InputNumber style={{ width: "100%" }} placeholder="Enter price" />
          </Form.Item>

          <Form.Item>
            <Button type="primary" htmlType="submit" style={{ width: "100%" }}>
              Submit
            </Button>
          </Form.Item>
        </Form>
      </div>
    </div>
  );
};

export default UploadForm;
