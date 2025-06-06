import '@ant-design/v5-patch-for-react-19';
import React, { useState } from 'react';
import { Form, Input, InputNumber, Upload, Button, notification } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import type { UploadFile, UploadChangeParam } from 'antd/es/upload/interface';
import 'antd/dist/reset.css';
import {apiClient} from './common'; // Adjust the import path as necessary

interface Product{
  name: string;
  price: number;
  description: string;
}

const UploadForm: React.FC = () => {
  const [form] = Form.useForm();
  // Explicitly type the fileList state as UploadFile[]
  const [fileList, setFileList] = useState<UploadFile[]>([]);

  const onFinish = async (values: any) => {
    const { name, price, description } = values;
    const productRequest: Product = {
      name,
      price: parseFloat(price),
      description,
    };
    try {
      // Use axios to POST formData to the API endpoint.
      // The validateStatus option is set to always true so you can handle non-2xx responses manually.
      const response = await apiClient.post('api/products', productRequest);
      console.log(response.data.title);
      if (response.status === 400) {
        notification.error({
          message: 'Bad Request',
          description: response.data.title,
        });
      } else if (response.status < 200 || response.status >= 300) {
        notification.error({
          message: 'Error',
          description: `request fail! with status ${response.status}, ${response.data.title}`,
        });
      } else{
        form.resetFields();
        setFileList([]);
        notification.success({
          message: 'Success',
          description: `request successfully!, at path: ${response.data.id}`,
        });
      }
    } catch (error: any) {
      console.error('Error:', error);
      notification.error({
        message: 'Error',
        description: 'An error occurred during request.',
      });
    }
  };

  // Allow Ant Design's Form.Item to properly extract the file list.
  const normFile = (e: any) => {
    if (Array.isArray(e)) {
      return e;
    }
    return e && e.fileList;
  };

  return (
    <Form form={form} onFinish={onFinish} layout="vertical">
      <Form.Item
        label="Name"
        name="name"
        rules={[{ required: true, message: 'Please input the product name!' }]}
      >
        <Input placeholder="Enter product name" />
      </Form.Item>
      <Form.Item
        label="Description"
        name="description"
        rules={[{ required: true, message: 'Please input the product description!' }]}
      >
        <Input placeholder="Enter product description" />
      </Form.Item>
      <Form.Item
        label="Price"
        name="price"
        rules={[{ required: true, message: 'Please input the price!' }]}
      >
        <InputNumber placeholder="Enter price" />
      </Form.Item>

      <Form.Item>
        <Button type="primary" htmlType="submit">
          Submit
        </Button>
      </Form.Item>
    </Form>
  );
};

export default UploadForm;
