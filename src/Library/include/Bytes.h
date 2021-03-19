#include <memory>

#pragma once

namespace Abstractions {
	/// <summary>
	///  Use this class to handle bytes array data
	/// </summary>
	class Bytes {
	private:
	    unsigned char* byteArray;
		unsigned int length;

		void copy(const unsigned char* byteArray, const unsigned int length);
	public:
		Bytes();
		Bytes(const unsigned char* byteArray, const unsigned int length);

		Bytes(const Bytes&);
		Bytes(Bytes&&) noexcept;
		Bytes operator=(const Bytes&);

		void SetFromArray(unsigned char*& byteArray, const unsigned int length);

		const unsigned char* GetBytes() const;

		const unsigned int GetLength() const;


		//todo: implement functions like GetReader() -> GetInt(), GetByte(), GetLong()
		~Bytes();
	};

	using BytesReference = std::shared_ptr<Bytes>;

}