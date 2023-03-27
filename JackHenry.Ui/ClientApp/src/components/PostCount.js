import React from "react";
import {
  Card,
  CardBody,
  CardTitle,
  CardSubtitle,
  Col,
  Row
} from "reactstrap";

const PostCount = (props) => {
  return (
      <Col md={4}>
        <Card>
          <CardBody>
            <CardTitle tag="h5">Post Count</CardTitle>
            <CardSubtitle className="mb-2 text-muted" tag="h6">
              Card subtitle
            </CardSubtitle>
            <Row>
              <p className="display-1 text-center">{props.postCount}</p>
            </Row>
          </CardBody>
        </Card>
      </Col>
  );
};

export default PostCount;
