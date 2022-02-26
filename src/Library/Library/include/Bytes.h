#include <memory>
#include <list>

#pragma once
namespace Abstractions {
	class TlvStructure;

	/// <summary>
	///  Use this class to handle bytes array data
	/// </summary>
	class Bytes {
	private:
	    unsigned char* byteArray;
		unsigned int length;

		void copy(const unsigned char* byteArray, const unsigned int length);
		void append(const unsigned char* byteArray, const unsigned int length);

	public:
		Bytes();
		Bytes(const unsigned char* byteArray, const unsigned int length);
		Bytes(const char);
		Bytes(const bool);
		Bytes(const int);
		Bytes(const long long);
		Bytes(const unsigned long long);
		Bytes(const Bytes&);
		Bytes(const TlvStructure&);
		Bytes(const std::list<TlvStructure>& tlvList);

		Bytes(Bytes&&) noexcept;
		Bytes operator=(const Bytes&);
		Bytes operator=(Bytes&&) noexcept;

		void SetFromArray(unsigned char*& byteArray, const unsigned int length);
		Bytes& Append(const unsigned char* byteArray, const unsigned int length);
		Bytes& Append(const Bytes& bytes);	
		Bytes& Append(const char);
		Bytes& Append(const int);
		Bytes& Append(const long long);
		Bytes& AppendListBreak();

		const unsigned char* GetBytes() const;

		const unsigned int GetLength() const;


		//todo: implement functions like GetReader() -> GetInt(), GetByte(), GetLong()
		~Bytes();
	};

	using BytesReference = std::shared_ptr<Bytes>;

}
