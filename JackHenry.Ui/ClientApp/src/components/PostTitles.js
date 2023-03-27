import React from "react";
import {
  Card,
  CardBody,
  CardTitle,
  CardSubtitle,
  Col,
  Table,
} from "reactstrap";

const PostTitles = (props) => {

  return (
    <Col md={4}>
      <Card>
        <CardBody>
          <CardTitle tag="h5">Posts</CardTitle>
          <CardSubtitle className="mb-2 text-muted" tag="h6">
            Newly fetched from the Plebbit API
          </CardSubtitle>
          <div className="tableFixHead">
            <Table striped bordered responsive>
              <thead>
                <tr>
                  <th>#</th>
                  <th>Title</th>
                </tr>
              </thead>
              <tbody>
                {props.postTitles.map((x, i) => (
                  <tr key={i}>
                    <th>{i}</th>
                    <td>{x}</td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </div>
        </CardBody>
      </Card>
    </Col>
  );
};

export default PostTitles;
