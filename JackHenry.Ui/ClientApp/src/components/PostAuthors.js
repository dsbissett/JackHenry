import React, { useState, useEffect, useRef } from "react";
import {
  Card,
  CardBody,
  CardTitle,
  CardSubtitle,
  Col,
  Row,
  Table,
} from "reactstrap";

const PostAuthors = (props) => {
  return (
    <Col md={4}>
      <Card>
        <CardBody>
          <CardTitle tag="h5">Authors</CardTitle>
          <CardSubtitle className="mb-2 text-muted" tag="h6">
            Le`Newly fetched authors from the Plebbit API
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
                {props.postAuthors.map((x, i) => (
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
export default PostAuthors;